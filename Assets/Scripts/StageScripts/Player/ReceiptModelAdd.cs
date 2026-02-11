using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiptModelAdd : MonoBehaviour
{
    /*
    void Start(){}
    void Update(){}
    */

    void AttachReceipt(GameObject newReceipt)
    {
        // 排出先のボーンを探す
        Transform exitBone = transform.Find("Root/Body/Receipt/Receipt.001/Receipt.002");

        // そのボーンの子要素に設定
        newReceipt.transform.SetParent(exitBone);

        // 位置と回転をゼロに密着させる
        newReceipt.transform.localPosition = Vector3.zero;
        newReceipt.transform.localRotation = Quaternion.identity;
    }
}
