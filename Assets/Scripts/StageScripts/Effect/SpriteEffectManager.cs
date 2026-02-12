using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEffectManager : MonoBehaviour
{
    public static SpriteEffectManager Instance;

    public GameObject NotionIconPrefab;

    void Awake()
    {
        if(Instance is null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayNotion(Vector3 position)
    {
        if(NotionIconPrefab is not null) Instantiate(NotionIconPrefab, position, Quaternion.identity);
    }
}
