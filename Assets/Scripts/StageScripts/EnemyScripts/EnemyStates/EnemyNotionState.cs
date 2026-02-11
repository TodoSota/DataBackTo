using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNotoinState : IEnemyState
{
    private EnemyController _enemy;

    // コンストラクタ
    public EnemyNotoinState(EnemyController enemy) => _enemy = enemy;
    private Timer _timer = new Timer();
    public void OnEnter()
    {
        _timer.Start(1.0f);
    }
    public void OnUpdate()
    {
        _timer.Update(Time.deltaTime);
        if(_timer.IsFinished) _enemy.ChangeState(_enemy.enemyAttackState);
    }

    public void OnExit()
    {
        
    }
}
