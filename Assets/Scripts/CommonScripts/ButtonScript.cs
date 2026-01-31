using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class ButtonScript : MonoBehaviour, ISelectHandler,
    IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    private const float DEFAULT_SCALE = 1f;
    private const float SELECTED_SCALE = 1.1f;


    // ボタンイベントに対するサイズ変更
    public void OnSelect(BaseEventData eventData)
    {
        ApplyScale(SELECTED_SCALE);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ApplyScale(SELECTED_SCALE);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        ApplyScale(DEFAULT_SCALE);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (EventSystem.current.currentSelectedGameObject != gameObject)
        {
            ApplyScale(DEFAULT_SCALE);
        }
    }

    /// <summary>
    /// 大きさをを引数にする
    /// </summary>
    /// <param name="scale"></param>
    private void ApplyScale(float scale)
    {
        transform.localScale = Vector3.one * scale;
    }
}
