using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalItem : MonoBehaviour
{
    private bool isReached = false;

    private void OnTriggerEnter(Collider other)
    {
        if(isReached) return;
        isReached = true;

        StageManager.Instance.ClearStage();
    }
}
