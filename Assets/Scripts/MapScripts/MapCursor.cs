using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCursor : MonoBehaviour
{
    public void Move(Vector3 pos)
    {
        transform.position = pos;
    }


    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void InActive()
    {
        gameObject.SetActive(false);
    }
}