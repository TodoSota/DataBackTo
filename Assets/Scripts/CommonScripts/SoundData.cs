using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundDataEntity
{
    public string key;
    public AudioClip clip;
    [Range(0, 1)] public float volume = 1f;
}

[CreateAssetMenu(fileName = "SoundData", menuName = "ScriptableObjects/SoundData")]
public class SoundData : ScriptableObject
{
    public List<SoundDataEntity> soundDataList;
}