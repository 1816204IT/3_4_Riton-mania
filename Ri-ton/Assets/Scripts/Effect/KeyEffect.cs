using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キー押下時のエフェクト表示クラス
/// </summary>
public class KeyEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer = null;
    private Color color = new Color();
    private float alpha = 0.0f;
    private float maxVisibleCnt = 0.0f;

    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.Log("nullを検知");
        }

        color = spriteRenderer.color;
    }

    void Update()
    {
        if (maxVisibleCnt > 0.0f)
        {
            maxVisibleCnt -= Time.deltaTime;
        }
        else
        {
            alpha = (alpha > 0.0f) ? alpha - 2.0f * Time.deltaTime : alpha;
        }
        spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
    }

    public void EffictStart()
    {
        alpha = 1.0f;
        maxVisibleCnt = 0.3f;
    }
}
