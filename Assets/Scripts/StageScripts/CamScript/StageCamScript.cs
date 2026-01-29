using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class StageCamScript : MonoBehaviour
{
    private CinemachineImpulseSource _impulsSource;
    void Start()
    {
        _impulsSource = GetComponent<CinemachineImpulseSource>();
    }
    public void ShakeCamera()
    {
        _impulsSource.GenerateImpulse();
    }
}
