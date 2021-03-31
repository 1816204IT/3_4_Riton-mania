using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// このスクリプトがアタッチされたオブジェクトを対象オブジェクトの
/// 位置に追従する
/// </summary>
public class RecttransformFollow : MonoBehaviour
{
    [SerializeField]
    private RectTransform target = null;

    private RectTransform rt = null;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        NullCheck();
    }

    void Update()
    {
        rt.position = target.position;
    }

    private void NullCheck()
    {
        target.IsNull(nameof(target));
        rt.IsNull(nameof(rt));
    }
}
