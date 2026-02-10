using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TransitionManager : MonoBehaviour
{
    private Canvas _myCanvas;
    private CodeGenerator _generator;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _BackGround;
    [SerializeField] private Image _code;
    [SerializeField] private float _coverDuration = 0.5f;
    [SerializeField] private float _delayTime = 0.2f;
    [SerializeField] private float _waitTime = 1f;
    [SerializeField] private float _wipeDuration = 0.5f;

    public static TransitionManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _myCanvas = GetComponent<Canvas>();
        _generator = GetComponent<CodeGenerator>();
    }

    public void StartTransition(string text, string sceneName)
    {
        _text.text = text;
        _generator.CreateCodeSlit(text);

        StartCoroutine(TransitionSequence(sceneName));
    }

    private IEnumerator TransitionSequence(string sceneName)
    {
        _myCanvas.enabled = true;
        StartCoroutine(AnimateCover(_BackGround, _coverDuration));
        yield return new WaitForSeconds(_delayTime);
        yield return StartCoroutine(AnimateCover(_code, _coverDuration));

        yield return new WaitForSeconds(_waitTime);
        SceneManager.LoadScene(sceneName);

        
        StartCoroutine(AnimateWipe(_BackGround, _wipeDuration));
        yield return new WaitForSeconds(_delayTime);
        yield return StartCoroutine(AnimateWipe(_code, _wipeDuration));
        _myCanvas.enabled = false;
    }

    private IEnumerator AnimateCover(Image target, float dulation)
    {
        target.fillOrigin = 0; 
        target.fillAmount = 0f;

        float elapsed = 0f;

        while (elapsed < dulation)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / dulation);

            target.fillAmount = t * t; 

            yield return null;
        }

        target.fillAmount = 1f;
        yield return null;
    }

    private IEnumerator AnimateWipe(Image target, float dulation)
    { 
        target.fillOrigin = 1;

        float elapsed = 0f;
        while (elapsed < dulation)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / dulation);
            
            target.fillAmount = 1f - t;
            yield return null;
        }
        target.fillAmount = 0f;

        yield return null;
    }
}
