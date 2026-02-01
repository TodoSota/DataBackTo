using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(EnemyStatus))]
[RequireComponent(typeof(EnvironmentSensor))]
public class EnemyController : MonoBehaviour
{
    // 敵のステータス
    public EnemyStatus Status;
    [SerializeField]public float ChaseRate = 2.0f;

    // ステートパターン用の現在のステート
    protected IEnemyState _currentState { get; set; }

    // 他のステートも、プロパティ（全体）が protected なら set も protected でOK
    public virtual IEnemyState enemyPatrolState { get; protected set; }
    public virtual IEnemyState enemyAttackState { get; protected set; }
    public virtual IEnemyState enemyCoolDownState { get; protected set; }
    public virtual IEnemyState enemyDamagedState { get; protected set; }
    public virtual IEnemyState enemyDieState { get; protected set; }

    // 目（プレイヤーの認識）
    public EnemyEye Eye;

    // 動作に関するクラス
    public EnemyMotor Motor;

    // 自身の物理情報
    public Rigidbody rb { get; protected set; }
    [field: SerializeField] public bool IsFlying { get; private set; } = false;

    // 攻撃のターゲット（プレイヤー）の位置情報
    public Transform Target { get; protected set; }
    
    // 最後に攻撃された場所
    public Vector3 LastHitPos { get; private set; }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Status = GetComponent<EnemyStatus>();
        Motor = GetComponent<EnemyMotor>();
        Eye.Init(transform);
        SetupStates();
    }

    protected virtual void Start()
    {
        // ステートを変更
        ChangeState(enemyPatrolState);
        Status.DieAction += TransitionToDie;
    }

    protected virtual void Update()
    {
        Target = Eye.CheckForPlayer();
        _currentState?.OnUpdate();
    }

    protected virtual void OnDestroy() => Status.DieAction -= TransitionToDie;

    /// <summary>
    /// 対応するステートを用意。
    /// 適宜オーバーライドすること
    /// </summary>
    protected virtual void SetupStates()
    {
        enemyPatrolState = new EnemyPatrolState(this);
        enemyAttackState = new EnemyAttackState(this);
        enemyCoolDownState = new EnemyCoolDownState(this);
        enemyDamagedState = new EnemyDamagedState(this);
        enemyDieState = new EnemyDieState(this);
    }

    /// <summary>
    /// 現在のステートを newState に変更
    /// </summary>
    /// <param name="newState">変更先のステート</param>
    public void ChangeState(IEnemyState newState)
    {
        if (newState == null || _currentState == newState) return;
        _currentState?.OnExit();
        _currentState = newState;
        Debug.Log($"<color=red>{gameObject.name}</color> が{newState.GetType().Name}状態へ移行！！");
        _currentState.OnEnter();
    }

    public virtual void Attack()=>Status.MainAttack?.ActionLogic.Execute(this, Status.MainAttack);
    public virtual void Patrol() => Motor.Patrol();
    public virtual void Move(Vector3 dir, float speed) => Motor.Move(dir, speed);

    public void TransitionToDie() => ChangeState(enemyDieState);

    public void TakeDamage(int amount, Vector3 AttackerPos)
    {
        LastHitPos = AttackerPos;
        Status.TakeDamage(amount);
        ChangeState(enemyDamagedState);
    }

    public virtual void DropItem()
    {
        // statusに持たせておきたいね
    }

    protected virtual void OnDrawGizmos()
    {
        Eye.DrawViewGizmos();
    }
}