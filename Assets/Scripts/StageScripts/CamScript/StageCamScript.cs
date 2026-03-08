using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.Universal;

public class StageCamScript : MonoBehaviour
{
    private CinemachineImpulseSource _impulsSource;
    public Color bgColor = Color.white;
    public LayerMask displayLayers;
    public Camera MainCamera; // ñ{ï®ÇÃÉJÉÅÉâ
    public Light clearLight;

    void Start()
    {
        _impulsSource = GetComponent<CinemachineImpulseSource>();
    }
    public void ShakeCamera()
    {
        _impulsSource.GenerateImpulse();
    }
    
    public void ShowClear()
    {
        MainCamera.clearFlags = CameraClearFlags.SolidColor;
        MainCamera.backgroundColor = bgColor;
        MainCamera.cullingMask = displayLayers;

        var urpCamData = MainCamera.GetComponent<UniversalAdditionalCameraData>();
        if (urpCamData != null)
        {
            urpCamData.renderPostProcessing = false;
        }
    }
}
