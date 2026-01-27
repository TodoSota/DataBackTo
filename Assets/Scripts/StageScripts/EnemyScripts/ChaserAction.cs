using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Enemy/ChaserAction")]
public class ChaserAction : AttackAction
{
    public override void Execute(EnemyControllerAbstract owner, AttackData data)
    {
        Vector3 direction = (owner.Target.position - owner.transform.position).normalized;
        owner.Move(direction, owner.speed * owner.chaseRate);
    }
}