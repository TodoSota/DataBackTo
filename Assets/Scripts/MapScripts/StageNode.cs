using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapScene
{
    public class StageNode : MonoBehaviour
    {
        // 隣接するステージ
        public StageNode upNode;
        public StageNode downNode;
        public StageNode leftNode;
        public StageNode rightNode;

        // ステージ情報
        public string stageName;
        public int StageIndex;
        [SerializeField] private bool isCleared;

        // オブジェクト情報
        private MeshRenderer _renderer;

        // マテリアル
        [SerializeField] private Material _clearedMaterial;
        [SerializeField] private Material _unclearedMaterial;

        void Start()
        {
            _renderer = GetComponent<MeshRenderer>();
        }

        // インデックスが何番目か？をマネージャーから教えるための関数
        public void SetIndex(int index)
        {
            StageIndex = index;
        }

        // Unityエディタ上で何か変更されたら
        void OnValidate()
        {
            // 右ノードを設定したら、そのノードの左に自分を登録
            if (rightNode != null && rightNode.leftNode != this)
            {
                rightNode.leftNode = this;
            }
            // 左ノードを設定したら、そのノードの右に自分を登録
            if (leftNode != null && leftNode.rightNode != this)
            {
                leftNode.rightNode = this;
            }
            // 上ノードを設定したら、そのノードの下に自分を登録
            if (upNode != null && upNode.downNode != this)
            {
                upNode.downNode = this;
            }
            // 下ノードを設定したら、そのノードの上に自分を登録
            if (downNode != null && downNode.upNode != this)
            {
                downNode.upNode = this;
            }

        }

        // クリア状況による色の変更
        void UpdateMaterial()
        {
            if (_renderer == null) return;

            Material mat = isCleared ? _clearedMaterial : _unclearedMaterial;
            _renderer.material = mat;
        }

        // クリア状態を変更
        public void SetClearFlag(bool flag)
        {
            isCleared = flag;
            UpdateMaterial();
        }

        // 自身のポジションを返す
        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}