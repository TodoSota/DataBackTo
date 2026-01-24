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

            _inputHandler.OnMoveInput += HandleMove;
            _inputHandler.OnConfirmInput += PlayTheStage;

            // 初期表示
            _view.UpdateView(StageNodes[_mapData.CurrentStageIndex]);
        }

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
        private void PlayTheStage()
        {
            // <最低限>
            Debug.Log("index : "+_mapData.CurrentStageIndex+" のステージを選択");
            
            // <追加>
            // 対応するステージをプレイ
            // シーン移動
            // 戻ってきたときは、カレントインデックスを指定すること
        }
    }
}