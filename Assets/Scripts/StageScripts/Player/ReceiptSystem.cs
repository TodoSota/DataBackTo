using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//
// MVP の "M" | レシート（セーブデータ）の保持と復元ロジック担当
//
public struct ReceiptData
{
    public float savedHp;
    public int savedMoney;
    public int savedJumpCount;
    public PlayerCondition savedCondition;

    public ReceiptData(float hp, int money, int jumpConut, PlayerCondition condition)
    {
        savedHp = hp;
        savedMoney = money;
        savedJumpCount = jumpConut;
        savedCondition = condition;
    }
}

public class ReceiptSystem : MonoBehaviour
{
    [Header("Settings")]
    public int maxReceiptLimit = 3;     // 最大保持数

    // レシート保存の格納場所
    public List<ReceiptData> receiptQueue = new List<ReceiptData>();

    private PlayerStatus status;

    // レシートのイベントUIの更新などに使用
    public UnityEvent<List<ReceiptData>> OnReceiptUpdate;

    void Start()
    {
        status = GetComponent<PlayerStatus>();
    }

    public void SaveState()
    {
        if (receiptQueue.Count >= maxReceiptLimit) return;  // 上限なら終了

        // 記録したデータを格納
        ReceiptData newData = new ReceiptData(status.hp, status.money, status.currentJumpCount, status.CurrentCondition);
        receiptQueue.Add(newData);

        // レシート上書きでのイベント発火
        OnReceiptUpdate?.Invoke(receiptQueue);

        UnityEngine.Debug.Log("Receipt Done!! : " + receiptQueue.Count);
    }

    public bool LoadState()
    {
        if (receiptQueue.Count <= 0) return false;    // 所持がなければ実行不可

        // 最新のデータを取り出す（元の仕様通りインデックス0を取得）
        int firstIndex = 0;
        ReceiptData data = receiptQueue[firstIndex];

        // PlayerStatus に値を書き戻す
        status.OverwriteHp(data.savedHp);
        status.OverWriteMoney(data.savedMoney);
        status.currentJumpCount = data.savedJumpCount;

        //
        // 保存内容に condition を追加したが、ここではまだ反映していない
        //

        // 使用済みのものは破棄
        receiptQueue.RemoveAt(firstIndex);

        // レシート上書きでのイベント発火
        OnReceiptUpdate?.Invoke(receiptQueue);

        UnityEngine.Debug.Log("Receipt is Used!! Current Num of : " + receiptQueue.Count);
        status.DisplayState();
        return true;
    }
}