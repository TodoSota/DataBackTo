using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReceiptUI : MonoBehaviour
{
    [System.Serializable]
    public class ConditionVisual
    {
        public PlayerCondition condition;
        public Sprite sprite;
        public Material material;
    }

    [SerializeField] private Image _displayImage;
    [SerializeField] private List<ConditionVisual> _visualSettings;
    [SerializeField] private TextMeshProUGUI _hpValue;
    [SerializeField] private TextMeshProUGUI _moneyValue;
    [SerializeField] private TextMeshProUGUI _jumpValue;

    public void UpdateDisplay(ReceiptData data)
    {
        ApplyValues(data.savedHp, data.savedMoney, data.savedJumpCount);
        SetColor(data.savedCondition);
    }
    public void ApplyValues(float hp, int money, int jump)
    {
        _hpValue.text = hp.ToString("F2");
        _moneyValue.text = money.ToString();
        _jumpValue.text = jump.ToString();
    }

    public void SetColor(PlayerCondition condition)
    {
        var setting = _visualSettings.Find(s => s.condition == condition);

        if (setting != null)
        {
            _displayImage.sprite = setting.sprite;
            _displayImage.material = setting.material;
        }
    }
}
