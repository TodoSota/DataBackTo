using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "MapData", menuName = "ScriptableObjects/MapData")]
public class MapData : ScriptableObject
{
    public string CurrentStageID;
    [System.Serializable]
    public class StageProgress
    {
        public string StageID;
        public bool IsCleared;
    }
    public List<StageProgress> StageProgressList = new List<StageProgress>();

    public void CompleteStage(string id)
    {
        var progress = StageProgressList.Find(p => p.StageID == id);
        if (progress != null) progress.IsCleared = true;
        else StageProgressList.Add(new StageProgress { StageID = id, IsCleared = true });
    }

    public bool GetIsCleared(string id)
    {
        var progress = StageProgressList.FirstOrDefault(p => p.StageID == id);
        return progress != null && progress.IsCleared;
    }

    public void SetClearFlag(string id)
    {
        var progress = StageProgressList.Find(p => p.StageID == id);
        if(progress is not null)
        {
            progress.IsCleared = true;
        }else
        {
            Debug.Log($"ステージ名 {id} は存在しません！");
        }
    }
}