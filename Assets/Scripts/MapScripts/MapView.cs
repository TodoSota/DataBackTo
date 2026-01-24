using UnityEngine;

namespace MapScene
{
    public class MapView : MonoBehaviour
    {
        [SerializeField] private MapCam _mapCam;
        [SerializeField] private MapCursor _cursor;
        [SerializeField] private StageNamePanel _stageNamePanel;
        [SerializeField] private float DISTANCE_FROM_NODE = 5f;

        // マップ上での位置表示を変更
        public void UpdateView(StageNode node)
        {
            Vector3 nodePos = node.GetPosition();
            
            // 各コンポーネントへの命令をまとめる
            _mapCam.Move(new Vector3(nodePos.x, DISTANCE_FROM_NODE, nodePos.z));
            _cursor.Move(nodePos);
            _stageNamePanel.SetStageName(node.stageName);
        }
    }
}