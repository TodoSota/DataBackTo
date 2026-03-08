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
    private GameObject[] ReceiptModels;             // レシートをまとめたもの

    [SerializeField] private Material FaceMaterial; // 顔のマテリアル
    [SerializeField] private Material DispMaterial; // もう一つのディスプレイのマテリアル

    // ライト
    [SerializeField] private Light MonitorSpotlight;
    [SerializeField] private Light MonitorPointLight;

    private bool LightisBoosted = false;
    private bool LightisPowered = false;
    private bool frag = false;

    private Rigidbody rb;
    private float horizontalInput;

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

    void Update()
    {
        OperateLight(); // Display に関するプレイヤーからの操作

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (frag)
            {
                FaceMaterial.SetVector("_ExpressionOffset", new Vector2(0.5f, 0f));
            }
            else
            {
                FaceMaterial.SetVector("_ExpressionOffset", new Vector2(0.5f, 0.5f));
            }
        }
        frag = !frag;
    }

    void LateUpdate()
    {
        ReceiptFlut();  // 移動によるレシートのたなびき
    }

    // ディスプレイのライトに関する操作
    private void OperateLight()
    {
        // 上矢印キーでライトの強弱を変更
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (LightisPowered)
            {
                DispMaterial.SetVector("_ExpressionOffset", new Vector2(0f, 0f));
                MonitorSpotlight.gameObject.SetActive(false);
                MonitorPointLight.gameObject.SetActive(false);
            }
            else
            {
                DispMaterial.SetVector("_ExpressionOffset", new Vector2(0.5f, 0f));
                MonitorSpotlight.gameObject.SetActive(true);
                MonitorPointLight.gameObject.SetActive(true);
            }
            LightisPowered = !LightisPowered;
        }
    }

    // 移動によるレシートのたなびき
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

    // レシートの増減の際の描画更新
    public void ReceiptReload(int viewnum)
    {
        // 一度全部非表示にする
        addModel1.SetActive(false);
        addModel2.SetActive(false);
        addModel3.SetActive(false);

        // 表示枚数分まで表示
        for (int i = 0; i < viewnum; i++)
        {
            ReceiptModels[i].SetActive(true);
        }
    }
}