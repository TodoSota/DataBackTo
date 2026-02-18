using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Phrase
{
    public Sprite Characterimage;
    [TextArea(3, 10)]
    public string Text;

    [Header("‘S•¶•\Ž¦Œã‚Ì‘Ò‚¿ŽžŠÔ")]
    [SerializeField] public float DisplayDuration = 1.0f;
}

[CreateAssetMenu(fileName = "NewConversation", menuName = "Conversation Data")]
public class PhrasesData : ScriptableObject 
{
    public List<Phrase> Phrases;
}