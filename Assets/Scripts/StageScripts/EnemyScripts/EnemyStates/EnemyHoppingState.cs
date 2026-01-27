using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHoppingState : IEnemyState
{
    private EnemyControllerAbstract _enemy;
    private IHoppable _enemyHopable;

    // コンストラクタ
    public EnemyHoppingState(EnemyControllerAbstract enemy){
        _enemy = enemy;
        _enemyHopable = enemy as IHoppable;
    }
    // Start is called before the first frame update
    public void OnEnter()
    {
    }
    public void OnUpdate()
    {
        if(!_enemy.IsGrounded()) return;
        _enemy.Move(_enemyHopable._hopDir, _enemyHopable.HopPower);
    }

    public void OnExit()
    {
        
    }
}
