using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : IEnemyState
{
    private EnemyControllerAbstract _enemy;

    // コンストラクタ
    public EnemyPatrolState(EnemyControllerAbstract enemy) => _enemy = enemy;
    public void OnEnter()
    {
    }
    public void OnUpdate()
    {
        if (_enemy.Target is not null)
        {
            Debug.Log("<color=red>enemy<color> : Playerを発見！！");
            _enemy.ChangeState(_enemy.enemyAttackState);
            return;
        }
        _enemy.Patrol();
    }

    public void OnExit()
    {
        
    }
}
