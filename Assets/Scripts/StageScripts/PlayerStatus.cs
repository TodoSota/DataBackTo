using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // 基本ステータス
    public float hp = 100f;
    public int money = 0;
    public const int MaxMoney = 50;
    public bool isDead = false;

    // 自動減少値
    public float hpLossPerSecond = 0.5f;

    // アクション管理
    public int currentJumpCount = 0;
    public int maxJumpLimit = 1;
    public bool isGrounded = true;

    // アクションイベント
    public event Action dieAction;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Mathf.Abs(rb.velocity.x) > 0.1f)
        {
            ConsumeHp(hpLossPerSecond * Time.deltaTime);
        }

        if (hp <= 0 && !isDead)
        {
            Die();
        }
    }

    public void DisplayState()
    {
        UnityEngine.Debug.Log("Hp : " + hp);
        UnityEngine.Debug.Log("JumpCount : " + currentJumpCount);
        UnityEngine.Debug.Log("Money : " + money);
    }

    public void ConsumeHp(float amount)
    {
        hp -= amount;
        hp = Mathf.Max(hp, 0);
    }

    public void InstantKill()
    {
        if (hp <= 0) return;

        hp = 0;
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UnityEngine.Debug.Log("Change Money" + amount);
        money = Mathf.Min(money, MaxMoney);
    }

    void Die()
    {
        UnityEngine.Debug.Log("Died!!");
        isDead = true;
        dieAction?.Invoke();
    }

    public bool Respawn()
    {
        var ReceiptSystem = GetComponent<ReceiptSystem>();
        bool done = ReceiptSystem.LoadState();
        
        if(done)isDead = false;
        return done;
    }

    public void ResetJumpConut()
    {
        currentJumpCount = 0;
    }
}
