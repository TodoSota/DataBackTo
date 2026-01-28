using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(EnemyStatus))]
[RequireComponent(typeof(EnvironmentSensor))]
public class EnemyControllerAbstract : MonoBehaviour
{
    // 敵のステータス
    public EnemyStatus Status;
    [SerializeField]public float ChaseRate = 2.0f;
    public float speed;

    // ステートパターン用の現在のステート
    protected IEnemyState _currentState { get; set; }

    // 他のステートも、プロパティ（全体）が protected なら set も protected でOK
    public virtual IEnemyState enemyPatrolState { get; protected set; }
    public virtual IEnemyState enemyAttackState { get; protected set; }
    public virtual IEnemyState enemyCoolDownState { get; protected set; }
    public virtual IEnemyState enemyDamagedState { get; protected set; }
    public virtual IEnemyState enemyDieState { get; protected set; }

    // 目（プレイヤーの認識）
    [SerializeField] protected EnemyEye _eye;
    // センサー
    protected EnvironmentSensor sensor;

    // 自身の物理情報
    public Rigidbody rb { get; protected set; }
    [field: SerializeField] public bool IsFlying { get; private set; } = false;

    // 攻撃のターゲット（プレイヤー）の位置情報
    public Transform Target { get; protected set; }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sensor = GetComponent<EnvironmentSensor>();
        _eye.Init(transform);
        SetupStates();
    }

    protected virtual void Start()
    {
        // ステートを変更
        ChangeState(enemyPatrolState);
        speed = Status.Speed;
    }

    protected virtual void Update()
    {
        Target = _eye.CheckForPlayer();
        _currentState?.OnUpdate();
    }

    /// <summary>
    /// 対応するステートを用意。
    /// 適宜オーバーライドすること
    /// </summary>
    protected virtual void SetupStates(){}

    public bool IsAtLedge() => sensor.IsAtLedge();
    public bool IsHittingWall() => sensor.IsHittingWall();
    public bool IsGrounded() => sensor.IsGrounded();

    /// <summary>
    /// 現在のステートを newState に変更
    /// </summary>
    /// <param name="newState">変更先のステート</param>
    public void ChangeState(IEnemyState newState)
    {
        if (newState == null) return;
        _currentState?.OnExit();
        _currentState = newState;
        Debug.Log($"<color=red>{gameObject.name}</color> が{newState.GetType().Name}状態へ移行！！");
        _currentState.OnEnter();
    }

    public virtual void Move(Vector3 dir, float speed){}
    public virtual void Patrol(){}

    public virtual void Attack()=>Status.MainAttack?.ActionLogic.Execute(this, Status.MainAttack);

    public virtual void Stop()
    {
        rb.velocity = Vector3.zero;
    }

    public virtual void DropItem()
    {
        // statusに持たせておきたいね
    }

    public virtual void FlipX()
    {
        transform.rotation *= Quaternion.Euler(0, 180, 0);
    }

    protected virtual void OnDrawGizmos()
    {
        _eye.DrawViewGizmos();
    }
}