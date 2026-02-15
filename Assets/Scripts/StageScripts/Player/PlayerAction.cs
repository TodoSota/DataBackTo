using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    // 通常攻撃
    public GameObject attackHitbox;     // attackRenge がオブジェクトとしてある
    public MeshRenderer attackRenderer; // 色を変えるためのレンダラー
    public float attackDuration = 0.5f; // 光る時間

    // 強化(射撃)攻撃
    public GameObject coinPrefab;
    public Transform firePoint;

    
    // ヒップドロップ
    public float hipdropForce = 20f;    // 落下速度
    public float hipdropDamage = 20f;   // 反動ダメージ
    private float ShortHipdropScale = 4.0f; // ヒップドロップ時の判定
    private bool isHipdropping = false;

    // 攻撃の種類
    public enum AttackType { Normal, HipDrop, SuperHipDrop }
    private AttackType currentAttackType;

    // 攻撃力 : 各種行動の際に適宜攻撃力を入力する
    [SerializeField] private int basePower = 1;
    private float GetAttackTypeMultiplier(AttackType type) => type switch
    {
        AttackType.Normal       => 1.0f,
        AttackType.HipDrop      => 10.0f,
        AttackType.SuperHipDrop => 50.0f,
        _                       => 1.0f
    };
    private float ConditionMultiplier => (status.CurrentCondition == PlayerCondition.Burn) ? 2.0f : 1.0f;
    private int TouchDamage => (status.CurrentCondition == PlayerCondition.Burn)? 1:5;

    // 他クラスや表示モデルに関するデータ
    private Rigidbody rb;
    private PlayerStatus status;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        status = GetComponent<PlayerStatus>();
        anim = GetComponentInChildren<Animator>();  // モデルのアニメーターを取得
        status.dieAction += OnPlayerDeath;
    }

    void Update()
    {
        // X - M キーで攻撃
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.C) || 
            Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.B) || 
            Input.GetKeyDown(KeyCode.N) || Input.GetKeyDown(KeyCode.M))
        {
            // 地上にいるならば通常攻撃
            if (status.isGrounded)
            {
                // 攻撃力を 1 に設定
                currentAttackType = AttackType.Normal;
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    // 強化(射撃)攻撃
                    if (status.money >= 1)
                    {
                        // お金があるなら発火
                        PerformShiftAttack();
                    } else
                    {
                        // ないなら通知
                        UnityEngine.Debug.Log("I have no money...");
                    }
                } else {
                    // 通常攻撃
                    StartCoroutine(PerformAttack());
                }
            }
            // 空中攻撃
            else
            {
                // 下入力があればヒップドロップ
                if(Input.GetKey(KeyCode.DownArrow)) StartCoroutine(PerformHipdrop());
                else
                {
                    currentAttackType = AttackType.Normal;
                    StartCoroutine(PerformAttack());
                }
            }
                
        }
    }

    private int GetFinalDamage(AttackType type) 
    {
        return Mathf.RoundToInt(basePower * GetAttackTypeMultiplier(type) * ConditionMultiplier);
    }

    // 通常攻撃のコルーチン
    IEnumerator PerformAttack()
    {
        if (!CanAttack()) yield break;
        attackHitbox.SetActive(true);                   // 攻撃範囲を表示
        anim.SetBool("isAttacking", true);              // アニメーターのパラメータ変更
        attackRenderer.material.color = Color.yellow;   // 色を黄色に
        yield return new WaitForSeconds(attackDuration);// 攻撃時間分
        attackHitbox.SetActive(false);                  // 攻撃範囲を非表示
        anim.SetBool("isAttacking", false);             // アニメーターのパラメータ変更
    }

    // ヒップドロップのコルーチン
    IEnumerator PerformHipdrop()
    {
        if (!CanAttack()) yield break;
        isHipdropping = true;
        anim.SetBool("isHipdropping", isHipdropping);    // アニメーターのパラメータ変更

        UnityEngine.Debug.Log("Shoooooot!!!");

        // 攻撃判定を直下に移動
        attackHitbox.transform.localPosition = new Vector3(0, -1.2f, 0);
        attackHitbox.SetActive(true);

        Vector3 defaultHitboxScale = attackHitbox.transform.localScale;
        if(status.CurrentCondition == PlayerCondition.Short){
            Vector3 currentScale = defaultHitboxScale;
            currentScale.x *= ShortHipdropScale;
            attackHitbox.transform.localScale = currentScale;
        }
        attackRenderer.material.color = Color.yellow;

        // 攻撃前にちょっと待機
        rb.velocity = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(attackDuration);

        // 強化行動のヒップドロップ
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && status.money >= 10)
        {
            currentAttackType = AttackType.SuperHipDrop;
            attackRenderer.material.color = Color.magenta;
            status.AddMoney(-10);
            rb.velocity = new Vector3(0, -hipdropForce * 2, 0);// 落下速度
            yield return new WaitUntil(() => status.isGrounded);// 着地待機
        }
        else // 通常時のヒップドロップ
        {
            currentAttackType = AttackType.HipDrop;
            rb.velocity = new Vector3(0, -hipdropForce, 0);// 落下速度
            yield return new WaitUntil(() => status.isGrounded);// 着地待機
            status.ConsumeHp(hipdropDamage);                // 着地自傷ダメージ
        }

        // 攻撃ダメージはヒットボックスの Trigger にある
        isHipdropping = false;
        anim.SetBool("isHipdropping", isHipdropping);    // アニメーターのパラメータ変更
        attackHitbox.SetActive(false);
        attackHitbox.transform.localScale = defaultHitboxScale;
        attackHitbox.transform.localPosition = new Vector3(1, 0, 0);
    }

    // 強化(射撃)攻撃
    void PerformShiftAttack()
    {
        if (!CanAttack()) return;
        status.AddMoney(-1);    // お金を消費
        // 弾を生成 | 第一引数：何を、第二引数：どこで、第三引数：どの向きで
        Instantiate(coinPrefab, firePoint.position, transform.rotation);
        UnityEngine.Debug.Log("Shoooooot!!!");
    }
    private bool CanAttack()
    {
        return !(status.isDead || rb.isKinematic);
    }

    // 攻撃判定の接触処理
    private void OnTriggerEnter(Collider other)
    {
        if (attackHitbox.activeSelf && other.CompareTag("Enemy"))
        {
            if (isHipdropping) UnityEngine.Debug.Log("Hit! HipDrop!");
            else UnityEngine.Debug.Log("Hit! Attack!");

            UnityEngine.Debug.Log("Hit! HipDrop!");UnityEngine.Debug.Log("Hit! Attack!");
            int damage = GetFinalDamage(currentAttackType);
            other.GetComponent<EnemyController>().TakeDamage(damage, transform.position);
        }
    }

    // 接触時の敵へのダメージ
    private void OnCollisionEnter(Collision collision)
    {
        GameObject target = collision.gameObject;
        if (target.CompareTag("Enemy"))
        {
            if(target.TryGetComponent<EnemyController>(out var enemy))
            {
                enemy.TakeDamage(TouchDamage, transform.position);
            }
        }
    }

    private void OnPlayerDeath()
    {
        Debug.Log("死んだぜ！！");
        StopAllCoroutines();
        attackHitbox.SetActive(false);
        attackHitbox.transform.localPosition = new Vector3(1, 0, 0);
        isHipdropping = false;
        anim.SetBool("isHipdropping", isHipdropping);
    }
}
