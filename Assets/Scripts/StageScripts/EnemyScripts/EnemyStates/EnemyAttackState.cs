using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    private EnemyController _enemy;

    // コンストラクタ
    public EnemyAttackState(EnemyController enemy) => _enemy = enemy;
    public void OnEnter()
    {
        _enemy.Eye.SpreadViewAngle();
    }
    public void OnUpdate()
    {
        // 見失ったら
        if(_enemy.Target is null)
        {
            _enemy.ChangeState(_enemy.enemyPatrolState);
            Debug.Log("<color=red>enemy</color> : 見失った...");
            return;
        }

        _enemy.Attack();
        _enemy.ChangeState(_enemy.enemyCoolDownState);

    }
    public void OnExit()
    {
    }
}
