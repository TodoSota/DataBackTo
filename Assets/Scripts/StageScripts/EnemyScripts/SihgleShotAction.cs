using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Enemy/SihgleShotAction")]
public class SihgleShotAction : AttackAction
{
    public override void Execute(EnemyControllerAbstract owner, AttackData data)
    {
        Vector3 dir = (owner.Target.position - owner.transform.position).normalized;

        // ’e‚Ì•ûŒü‚ğƒ^[ƒQƒbƒg‚ÉŒü‚¯‚Ä‰ñ“]
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // ’e‚Ìì¬ 
        GameObject bullet = Instantiate(data.BulletPrefab, owner.transform.position, rotation);

        // ’eƒf[ƒ^‚Ì’“ü
        NormalBullet script = bullet.GetComponent<NormalBullet>();
        script.SetUp(data);
    }
}