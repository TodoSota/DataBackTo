using System.Collections;
using System;
using UnityEngine;

//
// MVP の "V" |  攻撃関係での検知・判定を行う
//
public class PlayerCombat : MonoBehaviour
{
    // ==========================================
    // インスペクター設定
    // ==========================================
    [Header("Hitbox Settings")]
    [SerializeField] private GameObject attackHitbox;
    [SerializeField] private float attackDuration = 0.5f;

    [Header("Shoot Settings")]
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform firePoint;

    [Header("HipDrop Settings")]
    [SerializeField] private float hipdropForce = 20f;
    [SerializeField] private float hipdropDamage = 20f;
    private float shortHipdropScale = 4.0f;

    [Header("Damage Settings")]
    [SerializeField] private int basePower = 1;
    [SerializeField] private float hitDuration = 0.1f;

    // ==========================================
    // 状態と参照
    // ==========================================
    public enum AttackType { Normal, HipDrop, SuperHipDrop }
    public AttackType CurrentAttackType { get; private set; }
    public bool IsHipdropping { get; private set; } = false;

    private PlayerStatus status;
    private PlayerMovement movement;

    // 状態変化を知らせるためのイベント
    public event Action<Color> OnAttackColorChanged;
    public event Action<bool> OnAttackStateChanged;
    public event Action<bool> OnHipdropStateChanged;

    void Start()
    {
        status = GetComponent<PlayerStatus>();
        movement = GetComponent<PlayerMovement>();

        attackHitbox.SetActive(false);
    }

    // ==========================================
    // Presenterから呼ばれる実行メソッド
    // ==========================================

    // 通常攻撃
    public void ExecuteNormalAttack()
    {
        StartCoroutine(NormalAttackRoutine());
    }

    // 射撃
    public void ExecuteShoot()
    {
        Instantiate(coinPrefab, firePoint.position, transform.rotation);
        UnityEngine.Debug.Log("Shoooooot!!!");
    }

    // ヒップドロップ
    public void ExecuteHipDrop(bool isSuper)
    {
        StartCoroutine(HipDropRoutine(isSuper));
    }

    // 死亡時などの強制キャンセル用
    public void CancelAttacks()
    {
        StopAllCoroutines();
        attackHitbox.SetActive(false);
        attackHitbox.transform.localPosition = new Vector3(1, 0, 0);
        IsHipdropping = false;

        OnHipdropStateChanged?.Invoke(false);
        OnAttackStateChanged?.Invoke(false);
    }

    // ==========================================
    // 攻撃のコルーチン（時間の管理と当たり判定の操作）
    // ==========================================

    private IEnumerator NormalAttackRoutine()
    {
        CurrentAttackType = AttackType.Normal;

        attackHitbox.SetActive(true);
        OnAttackStateChanged?.Invoke(true);           // アニメーション用イベント
        OnAttackColorChanged?.Invoke(Color.yellow);   // 色変更用イベント

        yield return new WaitForSeconds(attackDuration);

        attackHitbox.SetActive(false);
        OnAttackStateChanged?.Invoke(false);
    }

    private IEnumerator HipDropRoutine(bool isSuper)
    {
        IsHipdropping = true;
        CurrentAttackType = isSuper ? AttackType.SuperHipDrop : AttackType.HipDrop;

        OnHipdropStateChanged?.Invoke(true);

        // 攻撃判定を直下に移動
        attackHitbox.transform.localPosition = new Vector3(0, -1.2f, 0);
        attackHitbox.SetActive(true);

        // Short(帯電)状態ならスケール変更
        Vector3 defaultScale = attackHitbox.transform.localScale;
        if (status.CurrentCondition == PlayerCondition.Short)
        {
            attackHitbox.transform.localScale = new Vector3(defaultScale.x * shortHipdropScale, defaultScale.y, defaultScale.z);
        }

        OnAttackColorChanged?.Invoke(isSuper ? Color.magenta : Color.yellow);

        // 攻撃前に空中でちょっと待機
        movement.Stop();
        yield return new WaitForSeconds(attackDuration);

        // 落下処理
        movement.ApplyDropForce(isSuper ? hipdropForce * 2 : hipdropForce);

        // 着地まで待機
        yield return new WaitUntil(() => movement.IsGrounded);

        // 通常ヒップドロップなら自傷ダメージ
        if (!isSuper)
        {
            status.ConsumeHp(hipdropDamage);
        }

        // 判定を元に戻す
        IsHipdropping = false;
        OnHipdropStateChanged?.Invoke(false);
        attackHitbox.SetActive(false);
        attackHitbox.transform.localScale = defaultScale;
        attackHitbox.transform.localPosition = new Vector3(1, 0, 0);
    }

    // ==========================================
    // ダメージ計算と衝突判定
    // ==========================================

    // ダメージ計算の係数になるものを計上
    private float GetAttackTypeMultiplier(AttackType type) => type switch
    {
        AttackType.Normal => 1.0f,
        AttackType.HipDrop => 10.0f,
        AttackType.SuperHipDrop => 50.0f,
        _ => 1.0f
    };

    // 状態異常に応じて ダメージ倍率 を変更
    private float ConditionMultiplier => (status.CurrentCondition == PlayerCondition.Burn) ? 2.0f : 1.0f;
    private int TouchDamage => (status.CurrentCondition == PlayerCondition.Burn) ? 1 : 5;

    // 係数などもろもろをまとめて最終数値計算
    private int GetFinalDamage(AttackType type)
    {
        return Mathf.RoundToInt(basePower * GetAttackTypeMultiplier(type) * ConditionMultiplier);
    }

    // ヒットボックスに敵が入る
    private void OnTriggerEnter(Collider other)
    {
        if (attackHitbox.activeSelf && other.CompareTag("Enemy"))
        {
            int damage = GetFinalDamage(CurrentAttackType);
            StageManager.Instance.PlayHitstop(hitDuration);
            other.GetComponent<EnemyController>()?.TakeDamage(damage, transform.position);
        }
    }

    // 敵と接触
    private void OnCollisionEnter(Collision collision)
    {
        GameObject target = collision.gameObject;
        if (target.CompareTag("Enemy"))
        {
            if (target.TryGetComponent<EnemyController>(out var enemy))
            {
                enemy.TakeDamage(TouchDamage, transform.position);
            }
        }
    }
}