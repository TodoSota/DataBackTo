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
        _enemy.Stop();
        _timer.Start(1.0f);

        PlayNotion();
    }

    public void OnUpdate()
    {
        _timer.Update(Time.deltaTime);
        if(_timer.IsFinished) _enemy.ChangeState(_enemy.enemyAttackState);
    }

    public void OnExit()
    {
        
    }

    private void PlayNotion()
    {
        SoundManager.Instance.PlaySE("Notion");

        Vector3 ownPos = _enemy.transform.position;
        Collider col = _enemy.GetComponent<Collider>();
        float offset = 1.0f;
        Vector3 headPos = new Vector3(ownPos.x, col.bounds.max.y + offset, ownPos.z);
        SpriteEffectManager.Instance.PlayNotion(headPos);
    }
}
