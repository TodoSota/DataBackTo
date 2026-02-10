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

        public static StageSelectManager Instance;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            // ゲームシーンから戻ってきた際は_mapData.CurrentStageIDを対応したものにする必要がある
            // スクリプタブルオブジェクトなどを用いてシーンをまたいだ管理が必要
            ApplyDataToNodes();
            if (string.IsNullOrEmpty(_mapData.CurrentStageID) || GetNodeByID(_mapData.CurrentStageID) == null)
            {
                if (StageNodes != null && StageNodes.Length > 0)
                {
                    _mapData.CurrentStageID = StageNodes[0].gameObject.name;
                    Debug.Log($"初期IDを設定しました: {_mapData.CurrentStageID}");
                }
            }
            _inputHandler.OnMoveInput += HandleMove;
            _inputHandler.OnConfirmInput += PlayTheStage;

            // 初期表示
            _view.UpdateView(GetNodeByID(_mapData.CurrentStageID));
        }

        private void HandleMove(Vector2Int direction)
        {
            StageNode currentNode = GetNodeByID(_mapData.CurrentStageID);
            StageNode nextNode = null;
        
            if (direction == Vector2Int.left) nextNode = currentNode.leftNode;
            else if (direction == Vector2Int.right) nextNode = currentNode.rightNode;
            else if (direction == Vector2Int.up) nextNode = currentNode.upNode;
            else if (direction == Vector2Int.down) nextNode = currentNode.downNode;

            if (nextNode == null) return;
            if(!(currentNode.IsCleared || nextNode.IsCleared)) return;

            _mapData.CurrentStageID = nextNode.name;
            _view.UpdateView(nextNode);
        }

        private StageNode GetNodeByID(string id)
        {
            return System.Array.Find(StageNodes, n => n.name == id);
        }

        private void ApplyDataToNodes()
        {
            foreach (var node in StageNodes)
            {
                // MapDataからIDをキーにしてクリア状況を取得
                bool cleared = _mapData.GetIsCleared(node.name);
                node.SetClearFlag(cleared);
            }
        }
        // 指定ノードのステージをプレイする
        private void PlayTheStage()
        {
            // <最低限>
            Debug.Log("index : "+_mapData.CurrentStageID+" のステージを選択");
            
            // <追加>
            // 対応するステージをプレイ
            // シーン移動
            // 戻ってきたときは、カレントインデックスを指定すること
            StageNode currentNode = GetNodeByID(_mapData.CurrentStageID);
            TransitionManager.Instance.StartTransition(_mapData.CurrentStageID, currentNode.StageName);
        }
    }
}