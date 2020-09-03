using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    private Text text = null;
    private float unitScore = 0;      // Good判定の時に加算されるスコア量(perfectは2倍)
    private float score = 0;

    [SerializeField]
    private TimingJudgment timingJudgment = null;

    void Start()
    {
        text = this.GetComponent<Text>();

        if (text == null || timingJudgment == null)
        {
            Debug.Log("nullを検知");
        }

        //0.1秒後にunitScoreを設定する
        Invoke("SetUnitScore", 0.1f);
    }

    private void SetUnitScore()
    {
        int maxComboNum = timingJudgment.GetMaxComboNum();
        if (maxComboNum == 0)
        {
            unitScore = 0;
        }
        else
        {
            unitScore = (1000000.0f / maxComboNum) / 2.0f;
        }
    }

    public void AddPerfect()
    {
        score += unitScore * 2.0f;
        ScoreOverCheck();
        int intScore = (int)(score);
        text.text = intScore.ToString();
    }

    public void AddGood()
    {
        score += unitScore;
        ScoreOverCheck();
        int intScore = (int)(score);
        text.text = intScore.ToString();
    }

    // スコアが100万点を超えていたら100万点にする
    private void ScoreOverCheck()
    {
        score = (score > 1000000.0f) ? 1000000.0f : score;
    }

    public int _score
    { 
        get { return (int)score; }
    }
}
