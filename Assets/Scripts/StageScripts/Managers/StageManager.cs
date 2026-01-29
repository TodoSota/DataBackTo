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
    private void Awake()
    {
        if(Instance is null) Instance = this;
        else Destroy(gameObject);
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
}
