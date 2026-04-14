using System;
using System.Collections;
using UnityEngine;


//
// MVP の "V" |  物理演算と移動担当
//
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public event Action OnLanded;

    // ==========================================
    // インスペクター設定（基本値と倍率）
    // ==========================================
    [Header("Speed Settings")]
    [SerializeField] private float defaultMoveSpeed = 5.0f;
    [SerializeField] private float ShortSpeedRate = 0.8f;
    [SerializeField] private float BurnSpeedRate = 1.5f;

    [Header("Jump Settings")]
    [SerializeField] private float defaultJumpForce = 5.0f;
    [SerializeField] private float ShortJumpRate = 0.8f;
    [SerializeField] private float BurnJumpRate = 1.5f;

    // ==========================================
    // 内部(Private)参照と状態
    // ==========================================
    private Rigidbody rb;
    private EnvironmentSensor sensor;

    // Presenterから毎フレーム渡される指示
    private float currentHorizontalInput;
    private PlayerCondition currentCondition = PlayerCondition.Normal;

    private bool isKnockBacking = false;

    // ==========================================
    // 外部（Presenter）に公開するプロパティ
    // ==========================================
    public Vector3 LastSafePosition { get; private set; }
    public bool IsGrounded { get; private set; }
    public float LocalVelocityY => transform.InverseTransformDirection(rb.velocity).y;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sensor = GetComponent<EnvironmentSensor>();
        SetUp();
    }

    // ==========================================
    // Presenterから呼ばれる命令群（コマンド）
    // ==========================================

    // 移動方向と状態の更新（毎フレーム Update から呼ばれる想定）
    public void SetMovementInput(float input, PlayerCondition condition)
    {
        currentHorizontalInput = input;
        currentCondition = condition;

        // 振り向き処理
        if (currentHorizontalInput > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (currentHorizontalInput < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    // ジャンプの実行
    public void ExecuteJump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, 0);                     // 落下中のジャンプ等を考慮し、Y軸の速度をリセット
        rb.AddForce(Vector3.up * GetCurrentJumpForce(), ForceMode.Impulse); // ジャンプとなる上方向への力の加算
        IsGrounded = false;
    }

    // ヒップドロップなどの急降下用
    public void ApplyDropForce(float force)
    {
        rb.velocity = new Vector3(0, -force, 0);
    }

    // ノックバックの実行
    public void ApplyKnockBack(Vector3 attackerPos)
    {
        Vector3 direction = transform.position - attackerPos;
        StopCoroutine(nameof(KnockBackSequence));
        StartCoroutine(KnockBackSequence(direction));
    }

    // 停止
    public void Stop()
    {
        rb.velocity = Vector3.zero;
    }

    // ワープ・復活時など地点移動に
    public void Warp(Vector3 position)
    {
        SetUp();
        rb.position = position;
    }

    // キネマティックをオフ(物理演算を停止)
    public void SetKinematic(bool isKinematic)
    {
        rb.isKinematic = isKinematic;
        if (isKinematic) Stop();
    }

    private void SetUp()
    {
        SetKinematic(false);
    }

    // ==========================================
    // 物理演算（自己完結する処理）
    // ==========================================

    // 移動など
    void FixedUpdate()
    {
        if (rb.isKinematic || isKnockBacking) return;

        UpdateGroundStatus();

        // 平面移動の適用
        rb.velocity = new Vector3(currentHorizontalInput * GetCurrentMoveSpeed(), rb.velocity.y, 0);
    }

    // 接地の判定・復活のため最後にいた地点を記録
    private void UpdateGroundStatus()
    {
        bool isGroundedNow = sensor.IsGrounded();

        if (isGroundedNow != IsGrounded)
        {
            IsGrounded = isGroundedNow;
            if (IsGrounded)
            {
                OnLanded?.Invoke();     // 接地イベントを発火
            }
        }

        if (isGroundedNow)
        {
            SaveLastSafepoint(transform.position);  // 安全な地点を記録
        }
    }

    // 復活のため、最後にいた場所を記録
    private void SaveLastSafepoint(Vector3 position)
    {
        LastSafePosition = position;
    }

    // ==========================================
    // 内部計算用メソッド
    // ==========================================

    private float GetCurrentMoveSpeed()
    {
        if (currentCondition == PlayerCondition.Short) return defaultMoveSpeed * ShortSpeedRate;
        if (currentCondition == PlayerCondition.Burn) return defaultMoveSpeed * BurnSpeedRate;
        return defaultMoveSpeed;
    }

    private float GetCurrentJumpForce()
    {
        if (currentCondition == PlayerCondition.Short) return defaultJumpForce * ShortJumpRate;
        if (currentCondition == PlayerCondition.Burn) return defaultJumpForce * BurnJumpRate;
        return defaultJumpForce;
    }

    // ノックバック処理
    private IEnumerator KnockBackSequence(Vector3 direction)
    {
        isKnockBacking = true;
        float kbX = (direction.x == 0f)
                ? (transform.right.x >= 0 ? -1f : 1f)
                : Mathf.Sign(direction.x);
        float hopY = 0.5f;
        Vector3 kbDir = new Vector3(kbX, hopY, 0f);

        // ノックバック直前に速度をリセットして安定させる
        rb.velocity = Vector3.zero;
        rb.AddForce(kbDir * 5f, ForceMode.VelocityChange);

        yield return new WaitForSeconds(0.2f);
        isKnockBacking = false;
    }
}