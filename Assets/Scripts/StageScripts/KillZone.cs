using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            var status = other.GetComponent<PlayerStatus>();
            status.InstantKill();
        }
    }
}
