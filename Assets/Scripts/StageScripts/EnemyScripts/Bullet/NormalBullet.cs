using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : BulletAbstract
{
    protected override void Update()
    {
        if(Data is null) return;
        transform.position += transform.right * Data.Speed * Time.deltaTime;
    }
}
