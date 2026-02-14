using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // public変数なら インスペクターから調整可能
    public float moveSpeed = 5.0f;
    public float jumpForce = 5.0f;

    // 外部からの情報を保持する
    private Rigidbody rb;
    private Animator anim;
    private PlayerStatus status;
    private EnvironmentSensor sensor;
    private float horizontalInput;

    // 復活に利用する安全地点の保存
    public Vector3 lastSafePosition { get; private set; }

    // ノックバック状態
    private bool isKnockBacking = false;

    // ダメージを受けた際の無敵状態・無敵時間
    public bool isInvincible;
    public float InvincibleTime = 1.0f;

    // 最初のフレームが始まるときに実行
    void Start()
    {
        rb = GetComponent<Rigidbody>();     // GameObject についている RigidBody を取得
        anim = GetComponentInChildren<Animator>();  // モデルのアニメーターを取得
        status = GetComponent<PlayerStatus>();
        sensor = GetComponent<EnvironmentSensor>(); // 接地情報などのセンサーを取得
        status.dieAction+=OnPlayerDeath;
        SetUp();
    }

    // 毎フレーム
    void Update()
    {
        
        horizontalInput = Input.GetAxisRaw("Horizontal");// 左右方向にキー入力を監視
        if(horizontalInput > 0 )
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // 右向き
        } else if (horizontalInput < 0 )
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // 左向き
        }

        // ジャンプ入力 : Space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    // 物理演算用のループ : 一定時間ごとに呼び出し
    void FixedUpdate()
    {
        if(rb.isKinematic || isKnockBacking) return;

        // 接地処理
        CheckGroundStatus();

        // 平面移動
        rb.velocity = new Vector3(horizontalInput * moveSpeed, rb.velocity.y, 0);
        // アニメーターのパラメータ変更
        anim.SetFloat("WalkSpeed", Mathf.Abs(horizontalInput * moveSpeed));
    }

    void Jump()
    {
        // 現在のジャンプ回数が既定の回数以下ならば
        if (status.currentJumpCount < status.maxJumpLimit)
        {
            // 上方向に力を加える
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            status.isGrounded = false;
        }
        else // 規定回数を超えていたら、コインを消費して発動
        {
            int cost = status.currentJumpCount + 1;
            if (status.money >= cost)
            {
                status.AddMoney(-cost);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                UnityEngine.Debug.Log("Extra Jump!!");
            }
        }


        // Status にジャンプ回数を加算
        status.currentJumpCount++;
    }

    void Stop()
    {
        rb.velocity = Vector3.zero;
    }

    void OnPlayerDeath()
    {
        rb.isKinematic = true;
    }

    void SetUp()
    {
        rb.isKinematic = false;
        Stop();
    }

    public void TakeDamage(float amount, Vector3 AttackerPos)
    {
        Vector3 direction = transform.position - AttackerPos;
        KnockBack(direction);
        status.ConsumeHp(amount);

        StartCoroutine(InvincibleCoroutine());
    }

    void KnockBack(Vector3 direction)
    { 
        StopCoroutine(nameof(KnockBackSequence));
        StartCoroutine(KnockBackSequence(direction));
    }

    IEnumerator KnockBackSequence(Vector3 direction)
    {
        isKnockBacking = true;
        float kbX = (direction.x == 0f) 
                ? (transform.right.x >= 0 ? -1f : 1f)
                : Mathf.Sign(direction.x);
        float hopY = 0.5f;
        Vector3 kbDir = new Vector3(kbX, hopY, 0f);
        rb.AddForce(kbDir * 5f, ForceMode.VelocityChange);

        yield return new WaitForSeconds(0.2f);

        isKnockBacking = false;
    }

    IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;
        StartBlinking();
        yield return new WaitForSeconds(InvincibleTime);
        isInvincible = false;
        EndBlinking();
    }

    private void StartBlinking()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.EnableKeyword("_EMISSION");

            renderer.material.SetColor("_EmissionColor", Color.white * 5f);
        }
    }

    private void EndBlinking()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.SetColor("_EmissionColor", Color.black);
            renderer.material.DisableKeyword("_EMISSION");
        }
    }

    public void Warp(Vector3 position)
    {
        Debug.Log("warp");
        SetUp();
        rb.position = position;
    }

    private void CheckGroundStatus()
    {
        bool isGroundedNow = sensor.IsGrounded();

        if (isGroundedNow != status.isGrounded)
        {
            status.isGrounded = isGroundedNow;
            anim.SetBool("isGround", status.isGrounded);
        }

        if (isGroundedNow)
        {
            status.ResetJumpConut();
            SaveLastSafepoint(transform.position);
        }
    }

    // 復活場所を保存
    // <追加>バグを防ぐなら、ステージ開始時にそのポジションを保存しておといいかも
    public void SaveLastSafepoint(Vector3 position)
    {
        lastSafePosition = position;
    }
}
