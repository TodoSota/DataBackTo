using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Enemy/ChaserAction")]
public class ChaserAction : AttackAction
{
    public override void Execute(EnemyController owner, AttackData data)
    {
        Vector3 direction = (owner.Target.position - owner.transform.position).normalized;
        owner.Move(direction, owner.Status.Speed * owner.ChaseRate);
    }
}