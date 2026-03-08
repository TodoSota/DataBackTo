using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] _seSources; // SE音源スピーカ
    [SerializeField] private AudioSource _bgmSource; // BGM音源スピーカ

    [SerializeField] private SoundData _seData;
    [SerializeField] private SoundData _bgmData;

    private Dictionary<string, SoundDataEntity> _seDict = new Dictionary<string, SoundDataEntity>();
    private Dictionary<string, SoundDataEntity> _bgmDict = new Dictionary<string, SoundDataEntity>();

    private string _currentBGMkey;
    public static SoundManager Instance;

    void Awake()
    {
        if(Instance is null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Register(_seData, _seDict);
            Register(_bgmData, _bgmDict);
        }
        else Destroy(gameObject);
    }

    private void Register(SoundData data, Dictionary<string, SoundDataEntity> dict)
    {
        foreach(var d in data.soundDataList)
        {
            if (!dict.ContainsKey(d.key)) dict.Add(d.key, d);
        }
    }

    public void PlaySE(string key)
    {
        if(!_seDict.TryGetValue(key, out var data))
        {
            Debug.Log("音源がないよ");
            return;
        }

        foreach (var source in _seSources)
        {
            if (!source.isPlaying)
            {
                source.clip = data.clip;
                source.volume = data.volume;
                source.loop = false;
                source.Play();
                return;
            }
        }
    }

    public void PlayBGM(string key)
    {
        if(!_bgmDict.TryGetValue(key, out var data))
        {
            Debug.Log("音源がないよ");
            return;
        }

        if(_currentBGMkey == key) return;
        _currentBGMkey = key;

        _bgmSource.clip = data.clip;
        _bgmSource.volume = data.volume;
        _bgmSource.loop = true; // ループ
        _bgmSource.Play();
        
        return;
    }

    public void StopBGM()
    {
        _bgmSource.Stop();
        _currentBGMkey = "";
    }
}