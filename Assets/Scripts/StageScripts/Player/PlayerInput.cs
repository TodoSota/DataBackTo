using System;
using UnityEngine;

//
// MVP の "V" | ユーザー操作の検知と通知担当
//
public class PlayerInput : MonoBehaviour
{
    // ==================================================
    // 状態として取得する入力（毎フレーム値が変わるもの）
    // ==================================================
    public float HorizontalInput { get; private set; }
    public bool IsShiftPressed { get; private set; }
    public bool IsDownArrowPressed { get; private set; }
    public bool IsEnterPressed { get; private set; }

    // ==================================================
    // ベントとして通知する入力（単発の操作）
    // ==================================================
    public event Action OnJumpPressed;         // Spaceキー
    public event Action OnAttackPressed;       // 攻撃キー (X, C, V, B, N, M)
    public event Action OnLightTogglePressed;  // 上矢印キー

    // レシート（エンターキー）操作関連
    public event Action OnReceiptSaveTriggered; // 長押し完了時
    public event Action OnReceiptLoadTriggered; // 短押し完了時（離した時）

    // --- 長押し判定用の設定・内部変数 ---
    [Header("Input Settings")]
    [SerializeField] private float receiptHoldRequiredTime = 0.2f;
    private float holdTimer = 0f;
    private bool isSaveProcessed = false;

    void Update()
    {

        HorizontalInput = Input.GetAxisRaw("Horizontal");                                       // 左右移動 入力の取得
        IsShiftPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);   // Shift    入力の取得
        IsDownArrowPressed = Input.GetKey(KeyCode.DownArrow);                                   // 下矢印   入力の取得
        ProcessReceiptInput();                                                                  // Enter    入力の取得
        // ジャンプ 入力の取得
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpPressed?.Invoke();
        }

        // 攻撃 入力の取得
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.C) ||
            Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.B) ||
            Input.GetKeyDown(KeyCode.N) || Input.GetKeyDown(KeyCode.M))
        {
            OnAttackPressed?.Invoke();
        }

        // 上矢印 入力の取得
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnLightTogglePressed?.Invoke(); // ライト切り替え
        }
    }

    // レシートの長押し・短押し判定ロジック
    private void ProcessReceiptInput()
    {
        // 押している間
        if (Input.GetKey(KeyCode.Return) && !isSaveProcessed)
        {
            IsEnterPressed = true;
            holdTimer += Time.deltaTime;
            if (holdTimer >= receiptHoldRequiredTime)
            {
                OnReceiptSaveTriggered?.Invoke(); // 長押し判定として通知
                isSaveProcessed = true;
                holdTimer = 0f;
            }
        }

        // 離した瞬間
        if (Input.GetKeyUp(KeyCode.Return))
        {
            IsEnterPressed = false;
            // セーブが処理されておらず、かつ一定時間以上〜規定時間未満なら短押し（ロード）と判定
            if (!isSaveProcessed && holdTimer > 0.1f && holdTimer < receiptHoldRequiredTime)
            {
                OnReceiptLoadTriggered?.Invoke(); // 短押し判定として通知
            }
            holdTimer = 0f;
            isSaveProcessed = false;
        }
    }
}