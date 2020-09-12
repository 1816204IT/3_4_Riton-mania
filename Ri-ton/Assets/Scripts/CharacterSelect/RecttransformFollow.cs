using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecttransformFollow : MonoBehaviour
{
    [SerializeField]
    private RectTransform target = null;

    private RectTransform rt = null;

    void Start()
    {
        rt = GetComponent<RectTransform>();

        if (target == null || rt == null)
        {
            Debug.Log("nullを検知");
        }
    }

    void Update()
    {
        rt.position = target.position;
    }
}
