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
    private PlayerStatus status;
    private float horizontalInput;

    // 復活に利用する安全地点の保存
    public Vector3 lastSafePosition { get; private set; }

    // 最初のフレームが始まるときに実行
    void Start()
    {
        // GameObject についている RigidBody を取得
        rb = GetComponent<Rigidbody>();
        status = GetComponent<PlayerStatus>();
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
        if(rb.isKinematic) return;
        // 平面移動
        rb.velocity = new Vector3(horizontalInput * moveSpeed, rb.velocity.y, 0);
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

    void KnockBack()
    {
        
    }

    public void Warp(Vector3 position)
    {
        Debug.Log("warp");
        SetUp();
        rb.position = position;
    }

    // 何かに接触したら
    private void OnCollisionStay(Collision collision)
    {
        // 接触しているオブジェクトの名前が Ground ならば
        if (collision.gameObject.CompareTag("Ground"))
        {
            status.isGrounded = true;
            status.ResetJumpConut();
            SaveLastSafepoint(transform.position);
        }
    }

    // 接触が離れたら
    private void OnCollisionExit(Collision collision)
    {
        // 接触しているオブジェクトの名前が Ground ならば
        if (collision.gameObject.CompareTag("Ground"))
        {
            status.isGrounded = false;
        }
    }

    // 復活場所を保存
    // <追加>バグを防ぐなら、ステージ開始時にそのポジションを保存しておといいかも
    public void SaveLastSafepoint(Vector3 position)
    {
        lastSafePosition = position;
    }
}
