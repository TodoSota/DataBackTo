using UnityEngine;

//
// MVP の "V" |  見た目の変更を行う
//
public class PlayerView : MonoBehaviour
{
    // ==========================================
    // インスペクター設定
    // ==========================================
    [Header("Animation")]
    [SerializeField] private Animator anim;

    [Header("Receipt Models")]
    public int scale = 2;
    [SerializeField] private GameObject[] receiptModels;    // レシートモデル。非表示/表示を変更する
    [SerializeField] private Transform targetBone;          // レシートの角度を決める根っこのボーン
    private Vector3 originEuler;

    [Header("Materials & Rendering")]
    [SerializeField] private Renderer playerRenderer;      // 無敵点滅用
    [SerializeField] private MeshRenderer attackRenderer;  // 攻撃判定の色変更用
    [SerializeField] private Material faceMaterial;
    [SerializeField] private Material dispMaterial;

    [Header("Lights")]
    [SerializeField] private Light monitorSpotlight;

    void Start()
    {
        // コンポーネントがアタッチされていなければ自動取得
        if (anim == null) anim = GetComponentInChildren<Animator>();
        if (playerRenderer == null) playerRenderer = GetComponent<Renderer>();

        originEuler = targetBone.localEulerAngles;

        // レシートの初期化処理
        foreach (var model in receiptModels)
        {
            if (model != null) model.SetActive(false);
        }
    }

    // ==========================================
    // アニメーション関連の命令群
    // ==========================================
    // アニメーターに "WalkSpeed" を入力。モーションに影響
    public void UpdateMoveAnimation(float speed)
    {
        anim.SetFloat("WalkSpeed", speed);
    }

    // アニメーターに "isGround" を入力。モーションに影響
    public void UpdateGroundAnimation(bool isGrounded)
    {
        anim.SetBool("isGround", isGrounded);
    }

    // アニメーターに "isAttacking" を入力。モーションに影響
    public void SetAttackAnimation(bool isAttacking)
    {
        anim.SetBool("isAttacking", isAttacking);
    }

    // アニメーターに "isHipdropping" を入力。モーションに影響
    public void SetHipDropAnimation(bool isHipdropping)
    {
        anim.SetBool("isHipdropping", isHipdropping);
    }

    // ==========================================
    // 描画・マテリアル関連の命令群
    // ==========================================
    public void SetAttackColor(Color color)
    {
        if (attackRenderer != null) attackRenderer.material.color = color;
    }

    public void StartBlinking()
    {
        if (playerRenderer != null)
        {
            playerRenderer.material.EnableKeyword("_EMISSION");
            playerRenderer.material.SetColor("_EmissionColor", Color.white * 5f);
        }
    }

    public void StopBlinking()
    {
        if (playerRenderer != null)
        {
            playerRenderer.material.SetColor("_EmissionColor", Color.black);
            playerRenderer.material.DisableKeyword("_EMISSION");
        }
    }

    // ==========================================
    // レジキャラ特有のギミック（レシート・ディスプレイ）
    // ==========================================

    // 「ローカルY軸速度」に応じて揺らす
    public void FlapReceipt(float localVelocityY)
    {
        float afterRotX = originEuler.x + localVelocityY * scale;
        afterRotX = Mathf.Clamp(afterRotX, -70, 100);

        targetBone.localEulerAngles = new Vector3(afterRotX, originEuler.y, originEuler.z);
    }

    // セーブ時などに Presenter から呼ばれる
    public void UpdateReceiptDisplay(int viewnum)
    {
        for (int i = 0; i < receiptModels.Length; i++)
        {
            receiptModels[i].SetActive(i < viewnum); // viewnum未満のインデックスだけtrueにする
        }
    }

    // モニターのライトの on/off 
    public void SetMonitorLight(bool isPowered)
    {
        if (isPowered)
        {
            dispMaterial.SetVector("_ExpressionOffset", new Vector2(0.5f, 0f));
            monitorSpotlight.gameObject.SetActive(true);
        }
        else
        {
            dispMaterial.SetVector("_ExpressionOffset", new Vector2(0f, 0f));
            monitorSpotlight.gameObject.SetActive(false);
        }
    }

    // 顔の表情の切り替え(UVスクロール)
    public void SetFaceExpression(bool isAlternative)
    {
        if (isAlternative)
            faceMaterial.SetVector("_ExpressionOffset", new Vector2(0.5f, 0f));
        else
            faceMaterial.SetVector("_ExpressionOffset", new Vector2(0.5f, 0.5f));
    }
}