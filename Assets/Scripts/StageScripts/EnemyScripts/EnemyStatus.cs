using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    // ザコ敵は基本的にライフ 1
    public int hp = 1;
    public float Speed = 2.0f;
    public float CoolTime = 3.0f;
    public AttackData MainAttack;

    // ダメージ処理
    public void TakeDamage(int damage)
    {
        hp -= damage;
        GetComponent<EnemyControllerAbstract>().ChangeState(
                GetComponent<EnemyControllerAbstract>().enemyDamagedState
            );
    }

    /*
    void Start(){}
    void Update(){}
    */
}
