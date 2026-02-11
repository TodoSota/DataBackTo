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

        StartBlinking();
        
        _timer = new Timer();
        _timer.Start(0.5f);
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
        EndBlinking();

        bool isFacing = _direction.x * _enemy.transform.right.x < 0;
        if(!isFacing) _enemy.Motor.FlipX();
    }

    private void StartBlinking()
    {
        var renderer = _enemy.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.EnableKeyword("_EMISSION");

            renderer.material.SetColor("_EmissionColor", Color.white * 5f);
        }
    }
    private void EndBlinking()
    {
        var renderer = _enemy.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.SetColor("_EmissionColor", Color.black);
            renderer.material.DisableKeyword("_EMISSION");
        }
    }
}
