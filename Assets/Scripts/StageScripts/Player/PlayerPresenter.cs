using System;
using UnityEngine;

//
// MVP の "P" |  Player にまつわる諸々全てにここで指示だし
//
public class PlayerPresenter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInput input;
    [SerializeField] private PlayerStatus status;
    [SerializeField] private PlayerView view;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerCombat combat;
    [SerializeField] private ReceiptSystem receiptSystem;

    // 内部状態の保持
    private bool isMonitorPowered = false;
    private bool isFaceAlternative = false;

    // イベント購読（Inputからの通知と実行する処理を紐づけ）
    private void Start()
    {
        // ===================================
        // 入力（Input）から
        // ===================================
        input.OnJumpPressed += HandleJump;
        input.OnAttackPressed += HandleAttack;
        input.OnLightTogglePressed += HandleLightToggle;
        input.OnReceiptSaveTriggered += HandleReceiptSave;
        input.OnReceiptLoadTriggered += HandleReceiptLoad;

        // ===================================
        // 攻撃（Combat）から描画（View）へ
        // ===================================
        combat.OnAttackStateChanged += view.SetAttackAnimation;
        combat.OnHipdropStateChanged += view.SetHipDropAnimation;
        combat.OnAttackColorChanged += view.SetAttackColor;

        // ===================================
        // ステータス（Status）
        // ===================================
        status.dieAction += HandleDeath;

        // ===================================
        // 移動（Movement）
        // ===================================
        movement.OnLanded += status.ResetJumpConut;

        // ===================================
        // 見た目へ（view）
        // ===================================
        receiptSystem.OnReceiptUpdate.AddListener(receipts => view.UpdateReceiptDisplay(receipts.Count));
    }

    // 毎フレームの処理
    private void Update()
    {
        if (status.isDead) return;

        // 移動入力と状態異常を Movement に渡す
        movement.SetMovementInput(input.HorizontalInput, status.CurrentCondition);

        // アニメーションの更新を View に渡す
        view.UpdateMoveAnimation(Mathf.Abs(input.HorizontalInput));
        view.UpdateGroundAnimation(movement.IsGrounded);
        view.FlapReceipt(movement.LocalVelocityY);
    }

    // =================================================
    // プレイヤーの入力からの発動事項 (ゲームのロジック)
    // =================================================

    // ジャンプロジック
    private void HandleJump()
    {
        if (status.isDead) return;

        // 規定回数までなら無償
        if (status.currentJumpCount < status.maxJumpLimit)
        {
            movement.ExecuteJump();
            status.currentJumpCount++;
        }
        else　// 規定回数を超えていたら、お金（ジャンプ回数+1）を消費して発動
        {
            int cost = status.currentJumpCount + 1;
            if (status.money >= cost)
            {
                status.AddMoney(-cost);
                movement.ExecuteJump();
                status.currentJumpCount++;
                Debug.Log("Extra Jump!!");
            }
        }

        // ジャンプしたときに表情変化発動!!...でも横からのビューなので見えないからいらない...
        isFaceAlternative = !isFaceAlternative;
        view.SetFaceExpression(isFaceAlternative);
    }

    // 攻撃ロジック
    private void HandleAttack()
    {
        if (status.isDead) return;

        // ① 地上にいる場合
        if (movement.IsGrounded)
        {
            if (input.IsShiftPressed)                   // Shift 押下判定
            {
                if (status.money >= 1)
                {
                    status.AddMoney(-1);
                    combat.ExecuteShoot();              // 射撃攻撃
                }
                else
                {
                    Debug.Log("I have no money...");    // 金なしなら Shift アクションはできない
                }
            }
            else
            {
                combat.ExecuteNormalAttack();           // 通常攻撃
            }
        }
        // ② 空中にいる場合
        else
        {
            if (input.IsDownArrowPressed)       // 下矢印キーを押下
            {
                // お金が10以上かつシフトキーなら強化ヒップドロップ
                bool isSuper = input.IsShiftPressed && status.money >= 10;
                if (isSuper) status.AddMoney(-10);

                combat.ExecuteHipDrop(isSuper);
            }
            else
            {
                combat.ExecuteNormalAttack();   // 通常攻撃
            }
        }
    }

    private void HandleLightToggle()
    {
        isMonitorPowered = !isMonitorPowered;
        view.SetMonitorLight(isMonitorPowered);
    }

    // レシート発行
    private void HandleReceiptSave()
    {
        StartCoroutine(view.PlayScanEffect(() =>
        {
            // エフェクトが完全に終わった後にこれが呼ばれる
            receiptSystem.SaveState();
        }));
    }

    // レシート使用
    private void HandleReceiptLoad()
    {
        receiptSystem.LoadState();
    }

    private void HandleDeath()
    {
        // 動きと攻撃を停止
        movement.SetKinematic(true);
        combat.CancelAttacks();

        // レシートがあるなら復活可能
        if (receiptSystem.receiptQueue.Count > 0)
        {
            Debug.Log("レシートを使って復活します！");

            receiptSystem.LoadState(); // レシート消費・HP復元
            status.Revive();           // 死亡フラグ解除

            if (RespawnManager.Instance != null)
            {
                RespawnManager.Instance.Respawn();
            }
            else
            {
                Debug.LogWarning("RespawnManagerが見つかりません！");
            }
        }
        else
        {
            // レシートがない場合、ゲームオーバー処理
            Debug.Log("Game Over...");
        }
    }

    private void OnDestroy()
    {
        // メモリリーク防止のため、オブジェクト破棄時にイベント購読を解除
        if (input != null)
        {
            input.OnJumpPressed -= HandleJump;
            input.OnAttackPressed -= HandleAttack;
            input.OnLightTogglePressed -= HandleLightToggle;
            input.OnReceiptSaveTriggered -= HandleReceiptSave;
            input.OnReceiptLoadTriggered -= HandleReceiptLoad;
        }

        if (combat != null)
        {
            combat.OnAttackStateChanged -= view.SetAttackAnimation;
            combat.OnHipdropStateChanged -= view.SetHipDropAnimation;
            combat.OnAttackColorChanged -= view.SetAttackColor;
        }

        if (movement != null)
        {
            movement.OnLanded -= status.ResetJumpConut;
        }

        if (receiptSystem != null)
        {
            receiptSystem.OnReceiptUpdate.RemoveAllListeners();
        }
    }
}