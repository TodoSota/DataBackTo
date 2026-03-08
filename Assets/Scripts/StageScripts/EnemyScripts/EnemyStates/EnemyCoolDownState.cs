using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCoolDownState : IEnemyState
{
    private EnemyController _enemy;
    private Timer timer;

    // コンストラクタ
    public EnemyCoolDownState(EnemyController enemy) => _enemy = enemy;
    public void OnEnter()
    {
        timer = new Timer();
        timer.Start(_enemy.Status.CoolTime);
    }
    public void OnUpdate()
    {
        timer.Update(Time.deltaTime);
        if (timer.IsFinished) _enemy.ChangeState(_enemy.enemyAttackState);
    }
    public void OnExit()
    {
        
    }
}
