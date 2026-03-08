using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEffect : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 1.0f;

    void Start()
    {
        Destroy(gameObject, _lifeTime);
    }

    void LateUpdate()
    {
        if(Camera.main is not null) transform.rotation = Camera.main.transform.rotation;
    }
}
