using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] private Respawner _respawner;
    [SerializeField] private GameObject _player;
    [SerializeField] private Vector3 _spawnPos = new Vector3(0f, 5f, 0f);
    [SerializeField] private Vector3 OFFSET = new Vector3(0f,1f,0);
    public static RespawnManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Respawn()
    {
        PlayerController playerController = _player.GetComponent<PlayerController>();
        Vector3 safePos = playerController.lastSafePosition;
        
        float halfHeight = _respawner.GetComponent<Renderer>().bounds.size.y / 2.0f;
        Vector3 spawnerPos = new Vector3(safePos.x, _spawnPos.y - halfHeight, safePos.z) + _spawnPos;

        _respawner.SetRespawner(spawnerPos);
        playerController.Warp(spawnerPos + OFFSET);
    }
}