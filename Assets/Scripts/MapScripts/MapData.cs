using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName = "ScriptableObjects/MapData")]
public class MapData : ScriptableObject
{
    public int CurrentStageIndex;

    public bool[] StageClearStatuses;

    // データをリセット（初期化）する機能を持っておくと便利
    public void Initialize(int stageCount)
    {
        CurrentStageIndex = 0;
        StageClearStatuses = new bool[stageCount];
    }
    // <追加>
    // 何かしらほかののステージデータを保持する場合
}