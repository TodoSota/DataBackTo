using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNotoinState : IEnemyState
{
    private EnemyController _enemy;

    // コンストラクタ
    public EnemyNotoinState(EnemyController enemy) => _enemy = enemy;
    public void OnEnter()
    {
    }
    public void OnUpdate()
    {
    }

    public void OnExit()
    {
        
    }
}
