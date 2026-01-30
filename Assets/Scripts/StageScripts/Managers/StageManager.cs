using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;
    [SerializeField] Animator clearAnime;
    [SerializeField] private float _waitTimeAfterShake = 2f;
    [SerializeField] private MapData _mapData;
    [SerializeField] private string _stageName;
    [SerializeField] private GameObject _player;
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

    void Start()
    {
        var PlayerStatus = _player.GetComponent<PlayerStatus>();
        PlayerStatus.dieAction+=OnDeath;
    }

    public void ClearStage()
    {
        Debug.Log("<color=yellow>ステージクリア</color>");
        StartCoroutine(ClearSequence());
    }

    private IEnumerator ClearSequence()
    {
        clearAnime.SetTrigger("Clear");
        yield return new WaitForSeconds(_waitTimeAfterShake);

        yield return SetClearFlag();

        SceneManager.LoadScene("MapScene");
    }

    private IEnumerator SetClearFlag()
    {
        _mapData.SetClearFlag(_stageName);
        yield return null;
    }

    public void OnDeath()
    {
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(3.0f);
        
        var status = _player.GetComponent<PlayerStatus>();
        
        bool respawned = status.Respawn();

        if(respawned)
        {
            RespawnManager.Instance.Respawn();
        }
        else
        {
            GameOverManager.Instance.GameOver();
        }
        yield return null;
    }
}