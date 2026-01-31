using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : MonoBehaviour, IBullet
{
    public AttackData Data { get; private set; }
    public Vector3 MoveDirection {get; private set; }

    public void SetUp(AttackData data, Transform target)
    {
        Data = data;
        
        if (Data.LifeTime > 0) Destroy(gameObject, Data.LifeTime);
    }
    void Update()
    {
        if(Data is null) return;
        transform.position += transform.right * Data.Speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other) {
        if ((Data.TargetLayers.value & (1 << other.gameObject.layer)) > 0)
        {
            if (other.TryGetComponent<PlayerController>(out var player))
            {
                //　<追加>ダメージ処理
                Debug.Log("<color=blue>Player<color>にダメージ!!");
            }

            // 当たったのが設定したレイヤーなら弾を消す
            Destroy(gameObject);
        }
    }
}
