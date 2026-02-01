using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;
    [SerializeField] private GameOverPanel _gameOverPanel;
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

    public void GameOver()
    {
        Debug.Log("game overだ");

        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        yield return ShowGameOverAnimation();

        ShowGameOverPanel();
        
        yield return new WaitUntil(() => _gameOverPanel.SelectedAction != GameOverAction.None);
        
        HideGameOverPanel();

        switch (_gameOverPanel.SelectedAction)
        {
            case GameOverAction.Continue:
                Continue();
                break;
            case GameOverAction.Title:
                BackToTitle();
                break;
        }
    }

    private IEnumerator ShowGameOverAnimation()
    {
        yield return null;
    }

    private void ShowGameOverPanel()
    {
        _gameOverPanel.Show();
    }

    private void HideGameOverPanel()
    {
        _gameOverPanel.Hide();
    }

    private void Continue()
    {
        Debug.Log("リスタート");
        StageManager.Instance.Continue();
    }

    private void BackToTitle()
    {
        Debug.Log("タイトルへ。　まだないけどね...");
    }
}