using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤの見た目に関する変更を行う
public class PlayerLookController : MonoBehaviour
{
    public int scale = 2;
    // レシートのモデル
    [SerializeField] private GameObject addModel1;
    [SerializeField] private GameObject addModel2;
    [SerializeField] private GameObject addModel3;
    [SerializeField] private Transform targetBone;  // レシートを接続する一番親のボーン
    private Vector3 originEuler;                    // 親ボーンの初期角度

    private GameObject[] ReceiptModels;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originEuler = targetBone.localEulerAngles;

        ReceiptModels = new GameObject[]{
            addModel1,
            addModel2,
            addModel3
        };

        ReceiptModels[0].SetActive(false);
    }

    void LateUpdate()
    {
        ReceiptFlut();
    }

    private void ReceiptFlut()
    {
        // 物体の移動速度に応じてレシートを傾ける
        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
        float localY = localVelocity.y;

        // 角度をクランプ
        float afterRotX = originEuler.x + localY * scale + 25;
        afterRotX = Mathf.Clamp(afterRotX, -30, 100); // モデルで固定なのでマジックナンバー

        // 変更した角度を格納
        targetBone.localEulerAngles =
        new Vector3(afterRotX,
                    originEuler.y,
                    originEuler.z);
    }

    public void ReceiptReload(int viewnum)
    {
        // 一度全部非表示にする
        addModel1.SetActive(false);
        addModel2.SetActive(false);
        addModel3.SetActive(false);

        // 表示枚数分まで表示
        for(int i=0; i<viewnum;  i++)
        {
            ReceiptModels[i].SetActive(true);
        }
    }
}
