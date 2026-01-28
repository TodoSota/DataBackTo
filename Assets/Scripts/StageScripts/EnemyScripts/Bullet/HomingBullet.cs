using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : MonoBehaviour, IBullet
{
    public AttackData Data { get; private set; }
    public Vector3 MoveDirection {get; private set; }
    private Transform _target;
    [SerializeField] private float turnSpeed = 45f;

    public void SetUp(AttackData data, Transform target)
    {
        Data = data;
        _target = target;
        
        if (Data.LifeTime > 0) Destroy(gameObject, Data.LifeTime);
    }

    void Update()
    {
        if(Data is null) return;
        Vector3 dir = (_target.position - transform.position).normalized;
        float deg = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.AngleAxis(deg, Vector3.forward);
        float maxDeg = turnSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, maxDeg);

        transform.position += transform.right * Data.Speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other) {
        if ((Data.TargetLayers.value & (1 << other.gameObject.layer)) > 0)
        {
            if (other.TryGetComponent<PlayerController>(out var player))
            {
                //　<追加>ダメージ処理
                Debug.Log("<color=blue>Player</color>にダメージ!!");
            }

            // 当たったのが設定したレイヤーなら弾を消す
            Destroy(gameObject);
        }
    }
}
