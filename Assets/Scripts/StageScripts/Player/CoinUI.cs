using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amountText;

    public void ApplyCoinAmount(int amount)
    {
        amountText.text = "Å~" + amount.ToString();
    }
}
