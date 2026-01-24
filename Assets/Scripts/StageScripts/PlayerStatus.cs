using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // 基本ステータス
    public float hp = 100f;
    public int money = 0;
    public const int MaxMoney = 50;

    // 自動減少値
    public float hpLossPerSecond = 0.5f;

    // アクション管理
    public int currentJumpCount = 0;
    public int maxJumpLimit = 1;
    public bool isGrounded = true;
    public float lastDirection = 1f;    // 現在移動している方向。体の向き 1 : -1


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

        if (hp <= 0)
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

    public void AddMoney(int amount)
    {
        money += amount;
        money = Mathf.Min(money, MaxMoney);
    }

    void Die()
    {
        UnityEngine.Debug.Log("Died!!");
    }

    public void ResetJumpConut()
    {
        currentJumpCount = 0;
    }
}
