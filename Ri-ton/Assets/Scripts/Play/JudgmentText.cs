using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// タイミング判定表示クラス
/// PERFECT,GOOD,MISSを表示する
/// </summary>
public class JudgmentText : MonoBehaviour
{
    private const float text_show_time = 1.0f;

    private Text text = null;
    private float time = 0.0f;

    void Start()
    {
        text = this.GetComponent<Text>();
        NullCheck();
    }

    void Update()
    {
        if (time >= text_show_time)
        {
            text.text = "";
            return;
        }

        time += Time.deltaTime;
    }

    /// <summary>
    /// Perfectを表示する
    /// </summary>
    public void PerfectJudgment()
    {
        time = 0.0f;
        text.color = Color.yellow;
        text.text = "PERFECT";
    }

    /// <summary>
    /// Goodを表示する
    /// </summary>
    public void GoodJudgment()
    {
        time = 0.0f;
        text.color = Color.blue;
        text.text = "GOOD";
    }

    /// <summary>
    /// Missを表示する
    /// </summary>
    public void MissJudgment()
    {
        time = 0.0f;
        text.color = Color.red;
        text.text = "MISS";
    }

    private void NullCheck()
    {
        text.IsNull();
    }
}
