using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    // ザコ敵は基本的にライフ 1
    public int hp = 1;

    // ダメージ処理
    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            UnityEngine.Debug.Log("Enemy Destroy!!");
            Destroy(gameObject);
        }
    }

    /*
    void Start(){}
    void Update(){}
    */
}
