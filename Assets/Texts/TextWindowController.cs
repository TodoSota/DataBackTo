using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextWindowController : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public Image CharaImgField;
    public static TextWindowController Instance;

    // テスト用のテキスト設置　ビルド時には消すこと
    [SerializeField] private PhrasesData testPhrases;

    void Awake()
    {
        if(Instance is null) Instance = this;
        else Destroy(gameObject);

        gameObject.SetActive(false);
    }

    public void StartConversation(List<Phrase> phrases)
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(PlayConversation(phrases));
    }

    public IEnumerator PlayConversation(List<Phrase> phrases)
    {
        foreach(Phrase phrase in phrases)
        {
            Text.text = "";
            CharaImgField.sprite = phrase.Characterimage;
            
            foreach(char letter in phrase.Text)
            {
                Text.text += letter;
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(phrase.DisplayDuration);
        }

        gameObject.SetActive(false);
    }
}
