using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapScene
{
    public class StageSelectManager : MonoBehaviour
    {
        public StageNode[] StageNodes;

        [SerializeField]private MapData _mapData;
        [SerializeField] private MapCam _mapCam; 
        [SerializeField] private float DISTANCE_FROM_NODE = 5f; // readonlyを消したもの
        // カーソル
        [SerializeField] private MapCursor cursor;
        // ステージの名前を表示するパネル
        [SerializeField] private StageNamePanel _stageNamePanel;

        void Start()
        {
            // ゲームシーンから戻ってきた際は_mapData.CurrentStageIndexを対応したものにする必要がある
            // スクリプタブルオブジェクトなどを用いてシーンをまたいだ管理が必要
            SetUpStageNodeIndex();
            ApplyDataToNodes();
            MoveAboveNode(_mapData.CurrentStageIndex);
        }

        void Update()
        {
            HandleArrowKeyInput();
            HandleReturnKeyInput();
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

        // 指定ノードへのカメラの移動
        public void MoveAboveNode(int index)
        {
            // ありえないインデックスをはじく
            if (index < 0 || index >= StageNodes.Length) return;

            //対象ノードのデータ取得
            StageNode node = StageNodes[index];
            Vector3 nodePos = node.GetPosition();
            
            // カメラ移動
            _mapCam.Move(new Vector3(nodePos.x, DISTANCE_FROM_NODE, nodePos.z));
            
            // カーソル移動
            cursor.Move(nodePos);

            // 現在地のステージ名の表示を更新
            _stageNamePanel.SetStageName(node.stageName);
        }

        // ステージ番号の割り振り
        public void SetUpStageNodeIndex()
        {
            for(int i = 0; i < StageNodes.Length; i++)
            {
                StageNodes[i].SetIndex(i);
            }
        }

        // 矢印キー入力による移動処理
        private void HandleArrowKeyInput()
        {
            // 現在ノードと次のノード
            StageNode nextNode = null;
            StageNode currentNode = StageNodes[_mapData.CurrentStageIndex];

            // キー入力先に隣接ノードがあるか？
            if (Input.GetKeyDown(KeyCode.LeftArrow)) nextNode = currentNode.leftNode;
            else if (Input.GetKeyDown(KeyCode.UpArrow)) nextNode = currentNode.upNode;
            else if (Input.GetKeyDown(KeyCode.RightArrow)) nextNode = currentNode.rightNode;
            else if (Input.GetKeyDown(KeyCode.DownArrow)) nextNode = currentNode.downNode;

            // あれば移動する
            if(nextNode is not null)
            {
                _mapData.CurrentStageIndex = nextNode.StageIndex;
                MoveAboveNode(_mapData.CurrentStageIndex);
            }
        }

        // リターン（Enterキー）によるステージ決定処理
        private void HandleReturnKeyInput()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                PlayTheStage(_mapData.CurrentStageIndex);
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