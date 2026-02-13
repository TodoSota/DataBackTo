using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ReceiptData
{
    public float savedHp;
    public int savedMoney;
    public int savedJumpCount;

    public ReceiptData(float hp, int money, int jumpConut)
    {
        savedHp = hp;
        savedMoney = money;
        savedJumpCount = jumpConut;
    }
}

public class ReceiptSystem : MonoBehaviour
{
    // 設定
    public float requireHoldTime = 0.5f;// セーブ発火に必要な時間
    public int maxReceiptLimit = 3;     // 最大保持数

    // レシート保存の格納場所
    public List<ReceiptData> receiptStack = new List<ReceiptData>();

    private PlayerStatus status;
    private PlayerLookController lookController;
    private float holdTimer = 0f;
    private bool isSaveProcessed = false;

    void Start()
    {
        status = GetComponent<PlayerStatus>();
        lookController = GetComponent<PlayerLookController>();
    }

    void Update()
    {
        // Enter キーを押している間
        if (Input.GetKey(KeyCode.Return) && !(isSaveProcessed))
        {
            // 長押しタイマー加算
            holdTimer += Time.deltaTime;

            if (holdTimer >= requireHoldTime)
            {
                SaveState();
                isSaveProcessed = true;
                holdTimer = 0f; // 重複セーブ防止のためリセット
            }
        }

        // キーを離した瞬間
        if (Input.GetKeyUp(KeyCode.Return))
        {
            if (!isSaveProcessed && holdTimer < requireHoldTime && holdTimer > 0.1f)
            {
                LoadState();
            }
            holdTimer = 0f;
            isSaveProcessed = false;
        }
    }

    void SaveState()
    {
        if (receiptStack.Count >= maxReceiptLimit) return;  // 上限なら終了

        // 記録したデータを格納
        ReceiptData newData = new ReceiptData(status.hp, status.money, status.currentJumpCount);
        receiptStack.Add(newData);

        // モデルの見た目を変更
        lookController.ReceiptReload(receiptStack.Count);

        UnityEngine.Debug.Log("Receipt Done!! : " + receiptStack.Count);
    }

    public bool LoadState()
    {
        if (receiptStack.Count <= 0) return false;    // 所持がなければ実行不可

        // 最新のデータを取り出す
        int lastIndex = receiptStack.Count - 1;
        ReceiptData data = receiptStack[lastIndex];

        // PlayerStatus に値を書き戻す
        status.OverwriteHp(data.savedHp);
        status.OverWriteMoney(data.savedMoney);
        status.currentJumpCount = data.savedJumpCount;

        // 使用済みのものは破棄
        receiptStack.RemoveAt(lastIndex);
        // モデルの見た目を変更
        lookController.ReceiptReload(receiptStack.Count);   

        UnityEngine.Debug.Log("Receipt is Used!! Current Num of : " + receiptStack.Count);
        status.DisplayState();
        return true;
    }
}