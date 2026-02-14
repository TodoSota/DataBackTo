using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiptContainer : MonoBehaviour
{
    [SerializeField] private ReceiptUI[] _receiptPool;

    void Awake()
    {
        foreach (var r in _receiptPool) r.gameObject.SetActive(false);
    }

    public void Sync(List<ReceiptData> receiptList)
    {
        for (int i = 0; i < _receiptPool.Length; i++)
        {
            if (i < receiptList.Count)
            {
                _receiptPool[i].gameObject.SetActive(true);
                        
                ReceiptData data = receiptList[i];

                _receiptPool[i].UpdateDisplay(data);
            }
            else
            {
                _receiptPool[i].gameObject.SetActive(false);
            }
        }
    }
}
