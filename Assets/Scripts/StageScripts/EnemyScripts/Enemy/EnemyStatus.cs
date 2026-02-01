using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyStatus : MonoBehaviour, IKillable
{
    // ザコ敵は基本的にライフ 1
    public int hp = 1;
    public float Speed = 2.0f;
    public float CoolTime = 3.0f;
    public AttackData MainAttack;

    private EnemyController _controller;
    public event Action DieAction;

    private void Awake() {
        _controller = GetComponent<EnemyController>();
    }
    // ダメージ処理
    public void TakeDamage(int damage)
    {
        hp -= damage;
    }

    public void InstantKill()
    {
        if (hp <= 0) return;

        hp = 0;
        DieAction?.Invoke();
    }


    /*
    void Start(){}
    void Update(){}
    */
}
