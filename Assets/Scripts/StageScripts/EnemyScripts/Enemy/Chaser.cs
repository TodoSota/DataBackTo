using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : EnemyControllerAbstract
{
    protected override void SetupStates()
    {
        enemyPatrolState = new EnemyPatrolState(this);
        enemyAttackState = new EnemyAttackState(this);
        enemyCoolDownState = new EnemyCoolDownState(this);
        enemyDamagedState = new EnemyDamagedState(this);
        enemyDieState = new EnemyDieState(this);
    }
    public override void Move(Vector3 dir, float speed)
    {
        rb.velocity = dir.normalized * speed;
        
        if (dir.x > 0.01f) transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (dir.x < -0.01f) transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public override void Patrol()
    {
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

    public override void Attack()
    {
        if (Target is null)
        {
            Debug.Log($"<color=red>{gameObject.name}<color> : Œ©Ž¸‚Á‚½");
            ChangeState(enemyPatrolState);
            return;
        }
        Status.MainAttack?.ActionLogic.Execute(this, Status.MainAttack);
    }


}