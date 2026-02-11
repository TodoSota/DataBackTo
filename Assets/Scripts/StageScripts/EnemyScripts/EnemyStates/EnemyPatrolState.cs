using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : IEnemyState
{
    private EnemyController _enemy;

    // コンストラクタ
    public EnemyPatrolState(EnemyController enemy) => _enemy = enemy;
    public void OnEnter()
    {
        _enemy.Eye.ResetViewAngle();
    }
    public void OnUpdate()
    {
        if (_enemy.Target is not null)
        {
            Debug.Log("<color=red>enemy</color> : Playerを発見！！");
            _enemy.ChangeState(_enemy.enemyAttackState);
            return;
        }
        _enemy.Patrol();
    }

    public void OnExit()
    {
        
    }
}
