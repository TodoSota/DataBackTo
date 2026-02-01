using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyDamagedState : IEnemyState
{
    private EnemyController _enemy;
    private Timer _timer;
    Vector3 _direction;

    // コンストラクタ
    public EnemyDamagedState(EnemyController enemy) => _enemy = enemy;
    public void OnEnter()
    {
        _direction = _enemy.transform.position - _enemy.LastHitPos;
        _enemy.Motor.KnockBack(_direction);
        
        _timer = new Timer();
        _timer.Start(0.3f);
    }
    public void OnUpdate()
    {
        _timer.Update(Time.deltaTime);

        if (!_timer.IsFinished) return;

        // 演出が終わってから
        if(_enemy.Status.hp > 0) _enemy.ChangeState(_enemy.enemyPatrolState);
        else _enemy.ChangeState(_enemy.enemyDieState);
    }
    public void OnExit()
    {
        bool isFacing = _direction.x * _enemy.transform.right.x < 0;
        if(!isFacing) _enemy.Motor.FlipX();
    }
}
