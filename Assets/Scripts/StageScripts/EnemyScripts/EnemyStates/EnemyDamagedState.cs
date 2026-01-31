using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamagedState : IEnemyState
{
    private EnemyControllerAbstract _enemy;

    // コンストラクタ
    public EnemyDamagedState(EnemyControllerAbstract enemy) => _enemy = enemy;
    public void OnEnter()
    {
        // ダメージ演出
        
    }
    public void OnUpdate()
    {
        // 演出が終わってから
        if(_enemy.Status.hp > 0) _enemy.ChangeState(_enemy.enemyPatrolState);
        else _enemy.ChangeState(_enemy.enemyDieState);
    }
    public void OnExit()
    {
        
    }
}
