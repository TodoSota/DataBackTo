using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Spark : MonoBehaviour
{
    [SerializeField] private VisualEffect _spark;

    void Start()
    {
        _spark = GetComponent<VisualEffect>();
        _spark.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _spark.Reinit();
            _spark.SendEvent("OnFire");
        }
    }
}
