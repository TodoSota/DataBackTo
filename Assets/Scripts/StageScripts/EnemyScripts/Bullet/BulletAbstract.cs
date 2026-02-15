using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletAbstract : MonoBehaviour
{
    public AttackData Data { get; protected set; }

    public virtual void SetUp(AttackData data, Transform target)
    {
        Data = data;
        if (Data.LifeTime > 0) Destroy(gameObject, Data.LifeTime);
    }

    protected virtual void Update(){}

    void OnTriggerEnter(Collider other) {
        if ((Data.TargetLayers.value & (1 << other.gameObject.layer)) > 0)
        {
            if (other.TryGetComponent<PlayerController>(out var player))
            {
                player.TakeDamage(Data.Damage, transform.position, Data.Condition);
                //　<追加>ダメージ処理
                Debug.Log("<color=blue>Player</color>にダメージ!!");
            }

            // 当たったのが設定したレイヤーなら弾を消す
            Destroy(gameObject);
        }
    }
}