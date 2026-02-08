using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecederMotor : EnemyMotor
{
    public override void Patrol()
    {
        if(!sensor.IsGrounded()) return;
        
        bool hasObstacle = sensor.IsHittingWall();

        if (hasObstacle)
        {
            FlipX();
            return;
        }

        Move(transform.right, status.Speed);
    }

    public override void Move(Vector3 dir, float speed)
    {
        Vector3 direction = dir;
        rb.velocity = direction.normalized * speed;
    }
}