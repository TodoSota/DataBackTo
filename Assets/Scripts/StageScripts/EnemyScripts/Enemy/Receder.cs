using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receder : EnemyControllerAbstract
{
    protected override void SetupStates()
    {
        enemyPatrolState = new EnemyPatrolState(this);
        enemyAttackState = new EnemyAttackState(this);
        enemyCoolDownState = new EnemyCoolDownState(this);
        enemyDamagedState = new EnemyDamagedState(this);
        enemyDieState = new EnemyDieState(this);
    }

    public override void Patrol()
    {
        if(!IsGrounded()) return;
        
        bool hasObstacle = IsHittingWall();

        if (!IsFlying)
        {
            hasObstacle |= IsAtLedge();
        }

        if (hasObstacle)
        {
            FlipX();
            return;
        }

        Move(transform.right, speed);
    }

    public override void Move(Vector3 dir, float speed)
    {
        Vector3 direction = dir;
        if(!IsFlying) direction.y = 0;
        rb.velocity = direction.normalized * speed;
    }
}