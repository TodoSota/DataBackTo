using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloaterMotor : EnemyMotor
{
    public Vector3 pointA = new Vector3(0,0,0);
    public Vector3 pointB = new Vector3(0,0,0);
    private Vector3 startPosition;

    private bool toPointA = true;

    [SerializeField] private bool pingPong = false;
    private WaypointPath _wayPointPath;
    private Vector3 vecAB;
    private Vector3 vecBA;

    protected override void Awake()
    {
        base.Awake();
        _wayPointPath = GetComponent<WaypointPath>();
        startPosition = transform.position;
        vecAB = (GetWorldB() - GetWorldA()).normalized;
        vecBA = (GetWorldA() - GetWorldB()).normalized;
    }

    public override void Patrol()
    {
        Vector3 destination = toPointA ? GetWorldA() : GetWorldB();
        float distance = Vector3.Distance(transform.position, destination);

        if(distance < 0.05f) 
        {
            toPointA = !toPointA;
        }
        Vector3 dir = toPointA ? vecBA : vecAB;

        Move(dir, status.Speed);
    }

    public override void Move(Vector3 dir, float speed)
    {
        Vector3 nextPosition = transform.position + (dir.normalized * speed * Time.fixedDeltaTime);
    
        rb.MovePosition(nextPosition);
    }

    private Vector3 GetWorldA() => _wayPointPath.GetWorldA();
    private Vector3 GetWorldB() => _wayPointPath.GetWorldB();

    // 移動を確認するための可視化
    protected void OnDrawGizmos()
    {
        if(pingPong) return;
        Gizmos.color = Color.cyan;
        Vector3 pos = Application.isPlaying ? startPosition : transform.position;
        // A地点とB地点を線で結ぶ
        Gizmos.DrawLine(pos + pointA, pos + pointB);
        Gizmos.DrawWireSphere(pos + pointA, 0.1f);
        Gizmos.DrawWireSphere(pos + pointB, 0.1f);
    }
}