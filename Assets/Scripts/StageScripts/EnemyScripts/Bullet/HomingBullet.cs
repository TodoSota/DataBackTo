using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet :BulletAbstract
{
    private Transform _target;
    [SerializeField] private float turnSpeed = 45f;

    public override void SetUp(AttackData data, Transform target)
    {
        _target = target;
        base.SetUp(data, target);
    }

    protected override void Update()
    {
        if(Data is null) return;
        Vector3 dir = (_target.position - transform.position).normalized;
        float deg = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.AngleAxis(deg, Vector3.forward);
        float maxDeg = turnSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, maxDeg);

        transform.position += transform.right * Data.Speed * Time.deltaTime;
    }
}
