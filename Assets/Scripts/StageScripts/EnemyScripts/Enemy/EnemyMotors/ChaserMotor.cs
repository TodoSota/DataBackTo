using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserMotor : EnemyMotor
{
    public override void Move(Vector3 dir, float speed)
    {
        Vector3 nextVelocity = dir.normalized * speed;

        rb.velocity = new Vector3(nextVelocity.x, rb.velocity.y, nextVelocity.z);
        
        if (dir.x > 0.01f) transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (dir.x < -0.01f) transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public override void Patrol()
    {
        bool hasObstacle = sensor.IsHittingWall();
        hasObstacle |= sensor.IsAtLedge();
        if (hasObstacle)
        {
            FlipX();
            return;
        }

        Move(transform.right, status.Speed);
    }
}