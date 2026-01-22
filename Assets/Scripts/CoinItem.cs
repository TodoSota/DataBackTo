using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItem : MonoBehaviour
{
    // コインの価値
    public int rewardAmount = 1;

    // トリガー内に入ったときに発火
    private void OnTriggerEnter(Collider collider)
    {
        // 接触相手が "Playerタグ"　であれば
        if (collider.CompareTag("Player"))
        {
            // 接触相手からコンポーネント取得
            PlayerStatus status = collider.GetComponent<PlayerStatus>();

            if (status != null)
            {
                // 相手のお金ステータスを増加
                status.AddMoney(rewardAmount);

                // 自分を画面から消去
                Destroy(gameObject);

                // <デバッグ用>
                status.displayState();
            }
        }
    }
}
