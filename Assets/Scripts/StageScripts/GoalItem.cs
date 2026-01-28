using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalItem : MonoBehaviour
{
    [SerializeField] private string MapScene = "MapScene";
    [SerializeField] private GameObject clearUI;

    private bool isReached = false;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(GoalSequence(other.gameObject));

        
    }

    private IEnumerator GoalSequence(GameObject player)
    {
        Debug.Log($"<color=Yellow>GOAL!!</color>");
        yield return null;
    }
}
