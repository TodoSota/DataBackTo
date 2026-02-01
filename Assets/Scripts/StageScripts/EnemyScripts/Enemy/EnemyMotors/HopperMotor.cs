using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HopperMotor : EnemyMotor
{
    [field:SerializeField] public float HopAngle{ get; private set; } = 45f;
    public Vector3 _hopDir { get; private set; }
    [field:SerializeField] public float HopPower { get; private set; } = 10.0f;
    protected override void Awake()
    {
        base.Awake();
        float x = Mathf.Cos(HopAngle);
        float y = Mathf.Sin(HopAngle);
        _hopDir = new Vector3(x, y, 0);
    }

    public override void Patrol()
    {
        if(!sensor.IsGrounded()) return;
        Move(_hopDir, HopPower);
    }

    public override void Move(Vector3 dir, float speed)
    {
        rb.velocity = dir.normalized * speed;
    }
}