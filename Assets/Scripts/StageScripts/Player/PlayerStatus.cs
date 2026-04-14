using System;
using UnityEngine;
using UnityEngine.Events;

//
// MVP の "M" | プレイヤーの基本ステータスと状態変化のルール担当
//
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
    private float hpLossMult => (CurrentCondition == PlayerCondition.Burn) ? burnDecayMult :
                       (CurrentCondition == PlayerCondition.Short) ? 0f :
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

    public void DisplayState()
    {
        UnityEngine.Debug.Log("Hp : " + hp);
        UnityEngine.Debug.Log("JumpCount : " + currentJumpCount);
        UnityEngine.Debug.Log("Money : " + money);
    }

    // ダメージ計算と適用
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

        OnHPChanged?.Invoke(hp / MAX_HP);

        // HPが減った瞬間に死亡判定
        if (hp <= 0 && !isDead)
        {
            Die();
        }
    }

    // 移動によるHP減少
    public void ApplyMovementPenalty(float deltaTime)
    {
        ConsumeHp(hpLossPerSecond * hpLossMult * deltaTime);
    }

    public void OverwriteHp(float value)
    {
        hp = value;
        OnHPChanged?.Invoke(hp / MAX_HP);
    }

    public void AbnormalConditionCheck(PlayerCondition condition)
    {
        if (condition == PlayerCondition.Normal) return;

        CurrentCondition = condition;
        OnConditionChanged?.Invoke(CurrentCondition);
        Debug.Log("状態異常を受けた");
    }

    // 場外落下などの即死
    public void InstantKill()
    {
        if (hp <= 0) return;
        hp = 0;
        ConsumeHp(0); // 死亡判定のため強制ゼロに
    }

    // 所持金の追加
    public void AddMoney(int amount)
    {
        money += amount;
        UnityEngine.Debug.Log("Change Money" + amount);
        money = Mathf.Min(money, MaxMoney);

        OnMoneyChanged?.Invoke(money);  // 変化イベントを通知
    }

    // 所持金の変更
    public void OverWriteMoney(int amount)
    {
        money = amount;
        OnMoneyChanged?.Invoke(money);  // 変化イベントを通知
    }

    // 死亡
    void Die()
    {
        UnityEngine.Debug.Log("Died!!");
        isDead = true;
        dieAction?.Invoke();
    }

    // 復活内部的な処理を戻すだけ、ステータスに関してはレシートへ
    public void Revive()
    {
        isDead = false;
    }

    // ジャンプ回数のリセット
    public void ResetJumpConut()
    {
        currentJumpCount = 0;
    }
}