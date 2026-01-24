using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapScene
{
    public class StageSelectManager : MonoBehaviour
    {
        public StageNode[] StageNodes;

        [SerializeField]private MapData _mapData;

        [SerializeField] private MapInputHandler _inputHandler;
        [SerializeField] private MapView _view;

        void Start()
        {
            // ゲームシーンから戻ってきた際は_mapData.CurrentStageIndexを対応したものにする必要がある
            // スクリプタブルオブジェクトなどを用いてシーンをまたいだ管理が必要
            SetUpStageNodeIndex();
            ApplyDataToNodes();

            // 入力イベントの登録
            // デリゲート
            _inputHandler.OnMoveInput += HandleMove;
            _inputHandler.OnConfirmInput += HandleConfirm;

            // 初期位置への移動
            _view.UpdateView(StageNodes[_mapData.CurrentStageIndex]);
        }

        // 移動入力をVectorの形で受け取り、対応した操作を行う。
        private void HandleMove(Vector2Int direction)
        {
            StageNode currentNode = StageNodes[_mapData.CurrentStageIndex];
            StageNode nextNode = null;

            if (direction == Vector2Int.left) nextNode = currentNode.leftNode;
            else if (direction == Vector2Int.right) nextNode = currentNode.rightNode;
            else if (direction == Vector2Int.up) nextNode = currentNode.upNode;
            else if (direction == Vector2Int.down) nextNode = currentNode.downNode;

            if (nextNode != null)
            {
                _mapData.CurrentStageIndex = nextNode.StageIndex;
                _view.UpdateView(nextNode);
            }
        }

        private void HandleConfirm()
        {
            Debug.Log($"Stage { _mapData.CurrentStageIndex } へ移動します");
        }

        private void ApplyDataToNodes()
        {
            // 配列のサイズが合わない場合
            if (_mapData.StageClearStatuses == null || _mapData.StageClearStatuses.Length != StageNodes.Length)
            {
                _mapData.Initialize(StageNodes.Length);
            }

            for (int i = 0; i < StageNodes.Length; i++)
            {
                StageNodes[i].SetClearFlag(_mapData.StageClearStatuses[i]);
            }
        }

        // ステージ番号の割り振り
        public void SetUpStageNodeIndex()
        {
            for(int i = 0; i < StageNodes.Length; i++)
            {
                StageNodes[i].SetIndex(i);
            }
        }

        // 指定ノードのステージをプレイする
        private void PlayTheStage(int index)
        {
            // <最低限>
            Debug.Log("index : "+index+" のステージを選択");
            
            // <追加>
            // 対応するステージをプレイ
            // シーン移動
            // 戻ってきたときは、カレントインデックスを指定すること
        }
    }
}