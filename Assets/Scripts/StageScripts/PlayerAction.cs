using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    // 通常攻撃
    public GameObject attackHitbox;     // attackRenge がオブジェクトとしてある
    public MeshRenderer attackRenderer; // 色を変えるためのレンダラー
    public float attackDuration = 0.2f; // 光る時間

    // ヒップドロップ
    public float hipdropForce = 20f;    // 落下速度
    public float hipdropDamage = 20f;   // 反動ダメージ
    private bool isHipdropping = false;

    private Rigidbody rb;
    private PlayerStatus status;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        status = GetComponent<PlayerStatus>();
    }

    void Update()
    {
        // 移動方向を記録
        float move = Input.GetAxisRaw("Horizontal");
        if (move != 0) status.lastDirection = move;

        // X - M キーで攻撃
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.C) || 
            Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.B) || 
            Input.GetKeyDown(KeyCode.N) || Input.GetKeyDown(KeyCode.M))
        {
            // 地上にいるならば通常攻撃
            if (status.isGrounded)
            {
                StartCoroutine(PerformAttack());
            }
            // 空中ならばヒップドロップ
            else
            {
                StartCoroutine(PerformHipdrop());
            }
                
        }
    }

    // 通常攻撃のコルーチン
    IEnumerator PerformAttack()
    {
        attackHitbox.SetActive(true);                   // 攻撃範囲を表示
        attackRenderer.material.color = Color.yellow;   // 色を黄色に
        yield return new WaitForSeconds(attackDuration);// 攻撃時間分
        attackHitbox.SetActive(false);                  // 攻撃範囲を非表示
    }

    // ヒップドロップのコルーチン
    IEnumerator PerformHipdrop()
    {
        isHipdropping = true;

        // 攻撃判定を直下に移動
        attackHitbox.transform.localPosition = new Vector3(0, -1.2f, 0);
        attackHitbox.SetActive(true);
        attackRenderer.material.color = Color.yellow;

        rb.velocity = new Vector3(0, -hipdropForce, 0);// 落下速度
        yield return new WaitUntil(() => status.isGrounded);// 着地待機
        status.ConsumeHp(hipdropDamage);                // 着地自傷ダメージ
        // 攻撃ダメージはヒットボックスの Trigger にある

        isHipdropping = false;
        attackHitbox.SetActive(false);
        attackHitbox.transform.localPosition = new Vector3(status.lastDirection, 0, 0);
        UnityEngine.Debug.Log("check");
    }

    // 攻撃判定の接触処理
    private void OnTriggerEnter(Collider other)
    {
        if (attackHitbox.activeSelf && other.CompareTag("Enemy"))
        {
            if (isHipdropping)
            {
                UnityEngine.Debug.Log("Hit! HipDrop!");
                other.GetComponent<EnemyStatus>().TakeDamage(10);
            }
            else
            {
                UnityEngine.Debug.Log("Hit! Attack!");
                other.GetComponent<EnemyStatus>().TakeDamage(1);
            }

        }
    }
}
