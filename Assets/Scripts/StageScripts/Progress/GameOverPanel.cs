using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _titleButton;
    public GameOverAction SelectedAction {get; private set;}
    public bool WasButtonPressed = false;
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        WasButtonPressed = false;
        SelectedAction = GameOverAction.None;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_continueButton.gameObject);

        _continueButton.onClick.RemoveAllListeners();
        _continueButton.onClick.AddListener(() => SelectedAction = GameOverAction.Continue);

        _titleButton.onClick.RemoveAllListeners();
        _titleButton.onClick.AddListener(() => SelectedAction = GameOverAction.Title);
    }
}

public enum GameOverAction {None, Continue, Title}
