using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieState : IEnemyState
{
    private EnemyControllerAbstract _enemy;

    // コンストラクタ
    public EnemyDieState(EnemyControllerAbstract enemy) => _enemy = enemy;
    public void OnEnter()
    {
        _enemy.Stop();
        _enemy.rb.isKinematic = true;
        var collider = _enemy.GetComponent<Collider>();
        if (collider != null) collider.enabled = false;

        Debug.Log("<color=red>Enemy : </color>雑魚は倒れた！");

        // 何かしらの演出を付けてもいい
        _enemy.DropItem();

        Destroy();
    }
    public void OnUpdate()
    {
        
    }
    public void OnExit()
    {
        
    }

    public void Destroy()
    {
        Object.Destroy(_enemy.gameObject);
    }
}
