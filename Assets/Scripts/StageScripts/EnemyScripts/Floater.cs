using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : EnemyControllerAbstract
{
    public Vector3 pointA = new Vector3(0,0,0);
    public Vector3 pointB = new Vector3(0,0,0);
    private Vector3 startPosition;

    [SerializeField] private bool pingPong = false;
    private WaypointPath _wayPointPath;

    protected override void Awake()
    {
        base.Awake();
        _wayPointPath = GetComponent<WaypointPath>();
        startPosition = transform.position;
        SetupStates();
    }

    protected override void SetupStates()
    {
        if (_wayPointPath)
        {
            enemyPatrolState = new EnemyPingPongState(this);
            enemyAttackState = new EnemyAttackState(this);
            enemyCoolDownState = new EnemyCoolDownState(this);
            enemyDamagedState = new EnemyDamagedState(this);
            enemyDieState = new EnemyDieState(this);
        }
        else
        {
            enemyPatrolState = new EnemyPatrolState(this);
            enemyAttackState = new EnemyAttackState(this);
            enemyCoolDownState = new EnemyCoolDownState(this);
            enemyDamagedState = new EnemyDamagedState(this);
            enemyDieState = new EnemyDieState(this);
        }
    }

    public override void Move(Vector3 dir, float speed)
    {
        Vector3 nextPosition = transform.position + (dir.normalized * speed * Time.fixedDeltaTime);
    
        rb.MovePosition(nextPosition);
    }

    public override void Attack()
    {
        base.Attack();
        ChangeState(enemyCoolDownState);
    }

    private void Shoot()
    {
        
    }

    // 移動を確認するための可視化
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if(pingPong) return;
        Gizmos.color = Color.cyan;
        Vector3 pos = Application.isPlaying ? startPosition : transform.position;
        // A地点とB地点を線で結ぶ
        Gizmos.DrawLine(pos + pointA, pos + pointB);
        Gizmos.DrawWireSphere(pos + pointA, 0.1f);
        Gizmos.DrawWireSphere(pos + pointB, 0.1f);
    }
}