using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    private AttackData _data;
    private Vector3 _moveDirection;

    public void SetUp(AttackData data)
    {
        _data = data;
        
        if (_data.LifeTime > 0) Destroy(gameObject, _data.LifeTime);
    }
    void Update()
    {
        if(_data is null) return;
        transform.position += transform.right * _data.Speed * Time.deltaTime;
    }

    private  void OnTriggerEnter(Collider other) {
        if ((_data.TargetLayers.value & (1 << other.gameObject.layer)) > 0)
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
