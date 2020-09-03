using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JudgmentText : MonoBehaviour
{
    private Text text = null;
    private int alpha = 255;

    void Start()
    {
        text = this.GetComponent<Text>();

        if (text == null)
        {
            Debug.Log("nullを検知");
        }
    }

    void Update()
    {
        if (alpha <= 0)
        {
            return;
        }

        alpha = (alpha - 2 < 0) ? 0 : alpha - 2;
        var color = text.color;
        text.color = new Color(color.r, color.g, color.b, alpha);
    }

    public void PerfectJudgment()
    {
        alpha = 255;
        text.color = new Color(255, 255, 0, alpha);
        text.text = "PERFECT";
    }

    public void GoodJudgment()
    {
        alpha = 255;
        text.color = new Color(0, 0, 255, alpha);
        text.text = "GOOD";
    }

    public void MissJudgment()
    {
        alpha = 255;
        text.color = new Color(255, 0, 0, alpha);
        text.text = "MISS";
    }
}
