using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPingPongState : IEnemyState
{
    private EnemyControllerAbstract _enemy;

    private bool toPointA = true;
    private WaypointPath _path;
    private Vector3 vecAB;
    private Vector3 vecBA;
    
    // コンストラクタ
    public EnemyPingPongState(EnemyControllerAbstract enemy) => _enemy = enemy;

    public void OnEnter()
    {
        _path = _enemy.GetComponent<WaypointPath>();
        if(_path is null) return;
        
        vecAB = (_path.GetWorldB() - _path.GetWorldA()).normalized;
        vecBA = (_path.GetWorldA() - _path.GetWorldB()).normalized;
    }
    public void OnUpdate()
    {
        if (_enemy.Target is not null)
        {
            Debug.Log("<color=red>enemy</color> : Playerを発見！！");
            _enemy.ChangeState(_enemy.enemyAttackState);
            return;
        }
        if(_path == null) return;

        Vector3 destination = toPointA ? _path.GetWorldA() : _path.GetWorldB();
        float distance = Vector3.Distance(_enemy.transform.position, destination);

        if(distance < 0.05f) 
        {
            toPointA = !toPointA;
        }
        Vector3 dir = toPointA ? vecBA : vecAB;

        _enemy.Move(dir, _enemy.speed);
    }

    public void OnExit()
    {
        
    }
}
