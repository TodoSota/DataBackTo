using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieState : IEnemyState
{
    private EnemyController _enemy;

    // コンストラクタ
    public EnemyDieState(EnemyController enemy) => _enemy = enemy;
    public void OnEnter()
    {
        Debug.Log("<color=red>Enemy : </color>雑魚は倒れた！");

        _enemy.Motor.DisablePhysics();

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
