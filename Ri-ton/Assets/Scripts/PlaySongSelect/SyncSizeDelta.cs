using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 指定のオブジェクトのサイズと同期する
/// </summary>
public class SyncSizeDelta : MonoBehaviour
{
    [SerializeField]
    private RectTransform targetRectTransform = null;
    private RectTransform myRectTransform;


    void Start()
    {
        myRectTransform = GetComponent<RectTransform>();
        if (myRectTransform == null || targetRectTransform == null)
        {
            Debug.Log("nullを検知");
        }
    }

    void Update()
    {
        myRectTransform.sizeDelta = targetRectTransform.sizeDelta;
    }
}
