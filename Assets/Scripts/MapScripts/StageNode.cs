using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageNode : MonoBehaviour
{
    // 隣接するステージ
    public StageNode upNode;
    public StageNode downNode;
    public StageNode leftNode;
    public StageNode rightNode;

    // ステージ情報
    public string stageName;
    [SerializeField] private bool isCleared;

    // オブジェクト情報
    private Transform myTransform;
    private MeshRenderer myRenderer;

    // マテリアル
    [SerializeField] private Material _clearedMaterial;
    [SerializeField] private Material _unclearedMaterial;

    void Start()
    {
        myTransform = transform;
        myRenderer = GetComponent<MeshRenderer>();

        UpdateMaterial();
    }

    // クリア状況による色の変更
    void UpdateMaterial()
    {
        if (myRenderer == null) return;

        Material mat = isCleared? _clearedMaterial : _unclearedMaterial;
        myRenderer.material = mat;
    }

    // クリア状態を変更
    public void SetClearFlag(bool flag)
    {
        this.isCleared = flag;
        UpdateMaterial();
    }

    // 自身のポジションを返す
    public Vector3 GetPosition()
    {
        return myTransform.position;
    }
}