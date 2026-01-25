using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
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


    // 最初のフレームが始まるときに実行
    void Start()
    {
        // GameObject についている RigidBody を取得
        rb = GetComponent<Rigidbody>();
        status = GetComponent<PlayerStatus>();
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

        // ジャンプ入力 : Space 、 接地時のみ呼び出し可能
        if (Input.GetKeyDown(KeyCode.Space) && status.currentJumpCount < status.maxJumpLimit)
        {
            Jump();
        }
    }

    // 物理演算用のループ : 一定時間ごとに呼び出し
    void FixedUpdate()
    {
        // 平面移動
        rb.velocity = new Vector3(horizontalInput * moveSpeed, rb.velocity.y, 0);
    }

    void Jump()
    {
        // 上方向に力を加える
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        status.isGrounded = false;

        // Status にジャンプ回数を加算
        status.currentJumpCount++;
    }

    // 何かに接触したら
    private void OnCollisionStay(Collision collision)
    {
        // 接触しているオブジェクトの名前が Ground ならば
        if (collision.gameObject.CompareTag("Ground"))
        {
            status.isGrounded = true;
            status.ResetJumpConut();
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
}
