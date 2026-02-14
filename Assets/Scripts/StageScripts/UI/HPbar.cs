using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HPbar : MonoBehaviour
{
    private Image img;
    void Awake()
    {
        img = GetComponent<Image>();
    }

    public void ApplyHP(float rate)
    {
        img.fillAmount = Mathf.Min(1, rate);
    }
}
