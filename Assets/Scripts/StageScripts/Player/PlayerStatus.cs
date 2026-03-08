using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatus : MonoBehaviour, IKillable
{
    // 基本ステータス
    public float hp = 100f;
    private float MAX_HP = 100f;
    public int money = 0;
    public const int MaxMoney = 50;
    public bool isDead = false;

    // 状態異常
    public PlayerCondition CurrentCondition = PlayerCondition.Normal;
    
    // 状態異常時のダメージ増加倍率
    public float shortDamageMult = 1.5f;
    public float burnDecayMult = 1.5f;

    // 自動減少値
    public float hpLossPerSecond = 0.5f;
    private float hpLossMult => (CurrentCondition == PlayerCondition.Burn)? burnDecayMult :
                       (CurrentCondition == PlayerCondition.Short)? 0f :
                        1f;

    // アクション管理
    public int currentJumpCount = 0;
    public int maxJumpLimit = 1;
    public bool isGrounded = true;

    // アクションイベント
    public event Action dieAction;
    public UnityEvent<float> OnHPChanged;
    public UnityEvent<int> OnMoneyChanged;
    public UnityEvent<PlayerCondition> OnConditionChanged;

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
            ConsumeHp(hpLossPerSecond * hpLossMult * Time.deltaTime);
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
    
    public void TakeDamage(float damage, PlayerCondition condition = PlayerCondition.Normal)
    {
        float shortMult = (CurrentCondition == PlayerCondition.Short) ? shortDamageMult : 1f;
        float amount = damage * shortMult;

        ConsumeHp(amount);

        AbnormalConditionCheck(condition);
    }

    public void ConsumeHp(float amount)
    {
        hp -= amount;
        hp = Mathf.Max(hp, 0);

        OnHPChanged?.Invoke(hp/MAX_HP);
    }

    public void OverwriteHp(float value)
    {
        hp = value;
        OnHPChanged?.Invoke(hp/MAX_HP);
    }

    public void AbnormalConditionCheck(PlayerCondition condition)
    {
        if(condition == PlayerCondition.Normal) return;

        CurrentCondition = condition;
        OnConditionChanged?.Invoke(CurrentCondition);
        Debug.Log("状態異常を受けた");
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

        OnMoneyChanged?.Invoke(money);
    }

    public void OverWriteMoney(int amount)
    {
        money = amount;
        OnMoneyChanged?.Invoke(money);
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
