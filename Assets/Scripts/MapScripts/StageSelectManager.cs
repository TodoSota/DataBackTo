using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapScene
{
    public class StageSelectManager : MonoBehaviour
    {
        // ステージの集合を管理
        public StageNode[] StageNodes;
        private int CurrentStageIndex = 0;
        // カメラとノードの間の距離
        [SerializeField] private readonly float DISTANCE_FROM_NODE = 5;

        // カメラ
        [SerializeField] private MapCam _mapCam;

        void Start()
        {
            // ゲームシーンから戻ってきた際はCurrentStageIndexを対応したものにする必要がある
            // スクリプタブルオブジェクトなどを用いてシーンをまたいだ管理が必要
            MoveCamAboveNode(CurrentStageIndex);
            SetUpStageNodeIndex();
        }

        void Update()
        {
            HandleArrowKeyInput();
            HandleReturnKeyInput();
        }

        // 指定ノードへのカメラの移動
        public void MoveCamAboveNode(int index)
        {
            if (index < 0 || index >= StageNodes.Length) return;
            Vector3 nodePos = StageNodes[index].GetPosition();
            _mapCam.Move(new Vector3(nodePos.x, DISTANCE_FROM_NODE, nodePos.z));
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
            StageNode nextNode = null;
            StageNode currentNode = StageNodes[CurrentStageIndex];

            if (Input.GetKeyDown(KeyCode.LeftArrow)) nextNode = currentNode.leftNode;
            else if (Input.GetKeyDown(KeyCode.UpArrow)) nextNode = currentNode.upNode;
            else if (Input.GetKeyDown(KeyCode.RightArrow)) nextNode = currentNode.rightNode;
            else if (Input.GetKeyDown(KeyCode.DownArrow)) nextNode = currentNode.downNode;

            if(nextNode is not null)
            {
                CurrentStageIndex = nextNode.StageIndex;
                MoveCamAboveNode(CurrentStageIndex);
            }
        }

        // リターン（Enterキー）によるステージ決定処理
        private void HandleReturnKeyInput()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                PlayTheStage(CurrentStageIndex);
            }
        }

        // 指定ノードのステージをプレイする
        private void PlayTheStage(int index)
        {
            Debug.Log("index : "+index+" のステージを選択");
            // 対応するステージをプレイ
            // シーン移動
            // 戻ってきたときは、カレントインデックスを指定すること
        }
    }
}