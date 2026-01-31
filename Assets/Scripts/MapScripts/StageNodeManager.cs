using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace MapScene
{
    public class StageNodeManager : MonoBehaviour
    {
        [SerializeField] private MapData mapData;
        [SerializeField] private StageSelectManager selectManager;

        [ContextMenu("Refresh Map Nodes")]
        public void RefreshMapNodes()
        {
            StageNode[] nodes = GetComponentsInChildren<StageNode>();

            selectManager.StageNodes = nodes;
            EditorUtility.SetDirty(selectManager);

            SyncMapData(nodes);

            EditorUtility.SetDirty(mapData);
            AssetDatabase.SaveAssets();

            Debug.Log($"同期完了：{nodes.Length} 個のステージを登録しました。");
        }

        private void SyncMapData(StageNode[] nodes)
        {
            // 既存のIDリストを取得
            var existingIDs = mapData.StageProgressList.Select(p => p.StageID).ToList();

            foreach (var node in nodes)
            {
                if (!existingIDs.Contains(node.name))
                {
                    mapData.StageProgressList.Add(new MapData.StageProgress 
                    { 
                        StageID = node.name, 
                        IsCleared = false 
                    });
                }
            }
        }

        private void OnValidate()
        {
            EditorApplication.delayCall += () => {
                if (this == null) return;
                RefreshMapNodes();
            };
        }
    }
}
