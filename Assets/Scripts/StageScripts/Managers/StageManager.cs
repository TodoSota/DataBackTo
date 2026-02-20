using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using MapScene;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;
    [SerializeField] private PlayableDirector _clearDirector;
    [SerializeField] private Image _stageClear;
    [SerializeField] private float _waitTimeClearEffect = 0.5f;
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
        PlayerStatus.dieAction+=OnPlayerDeath;
    }

    public void ClearStage()
    {
        Debug.Log("<color=yellow>ステージクリア</color>");
        StartCoroutine(ClearSequence());
    }

    private IEnumerator ClearSequence()
    {
        DestroyObjectsByTag("Enemy");
        DestroyObjectsByTag("Projectile");
        Debug.Log("Clear");

        PlaySlowMotion(_waitTimeClearEffect);
        yield return new WaitForSeconds(_waitTimeClearEffect);

        _clearDirector.Play();
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        _stageClear.gameObject.SetActive(false);
        yield return SetClearFlag();

        TransitionManager.Instance.StartTransition("Stage Map", "MapScene");
    }
    
    private void DestroyObjectsByTag(string tagName)
    {
        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag(tagName);
        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }
    }
    private IEnumerator SetClearFlag()
    {
        _mapData.SetClearFlag(_stageName);
        yield return null;
    }

    public void Continue()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void OnPlayerDeath()
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

    public void PlayHitstop(float duration)
    {
        StartCoroutine(HitstopCoroutine(duration));
    }

    private IEnumerator HitstopCoroutine(float duration)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }

    public void PlaySlowMotion(float duration, float slowScale = 0.2f)
    {
        StartCoroutine(SlowMotionCoroutine(duration, slowScale));
    }

    private IEnumerator SlowMotionCoroutine(float duration, float slowScale)
    {
        Time.timeScale = slowScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}