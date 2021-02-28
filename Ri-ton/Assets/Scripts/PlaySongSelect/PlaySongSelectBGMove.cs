using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 曲選択画面の背景画像を動かす
/// </summary>
public class PlaySongSelectBGMove : MonoBehaviour
{
    [SerializeField]
    private float rotRadius = 10.0f;
    [SerializeField]
    private float rotSpeed =1.0f;     // 1秒に回転する角度

    private float nowRot = 0.0f;
    private bool isRotAdd = true;   // 現在+の方向に回転しているか

    private RectTransform rt = null;

    void Start()
    {
        rt = GetComponent<RectTransform>();

        if (rt == null)
        {
            Debug.Log("nullを検知");
        }
    }

    void Update()
    {
        var r = rt.rotation.eulerAngles;
        if (isRotAdd)
        {
            r.z += rotSpeed * Time.deltaTime;
            rt.rotation = Quaternion.Euler(r);

            if (r.z >= rotRadius && r.z < 180)
            {
                isRotAdd = false;
            }
        }
        else
        {
            r.z -= rotSpeed * Time.deltaTime;
            rt.rotation = Quaternion.Euler(r);

            if (r.z <= 360 - rotRadius && r.z > 180)
            {
                isRotAdd = true;
            }
        }
    }
}
