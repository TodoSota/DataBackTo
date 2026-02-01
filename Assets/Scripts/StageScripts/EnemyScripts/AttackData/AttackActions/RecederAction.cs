using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Enemy/RecederAction")]
public class RecederAction : AttackAction
{
    public override void Execute(EnemyController owner, AttackData data)
    {
        Vector3 dir = (owner.Target.position - owner.transform.position).normalized;
        owner.Move(-dir, owner.Status.Speed * owner.ChaseRate);

        // ’e‚Ì•ûŒü‚ğƒ^[ƒQƒbƒg‚ÉŒü‚¯‚Ä‰ñ“]
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // ’e‚Ìì¬
        GameObject bullet = Instantiate(data.BulletPrefab, owner.transform.position, rotation);

        // ’eƒf[ƒ^‚Ì’“ü
        BulletAbstract script = bullet.GetComponent<BulletAbstract>();
        Transform target = owner.Target;
        script.SetUp(data, target);
    }
}