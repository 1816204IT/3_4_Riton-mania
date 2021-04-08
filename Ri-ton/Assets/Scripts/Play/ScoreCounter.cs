using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スコア計算クラス
/// </summary>
public class ScoreCounter : MonoBehaviour
{
    [SerializeField]
    private TimingJudgment timingJudgment = null;
    [SerializeField]
    private AccCounter accCounter = null;

    private Text text = null;
    private float unitScore = 0;      // Good判定の時に加算されるスコア量(perfectは2倍)
    private float score = 0;

    void Start()
    {
        text = this.GetComponent<Text>();
        NullCheck();

        //0.1秒後にunitScoreを設定する
        Invoke("SetUnitScore", 0.1f);
    }

    private void Update()
    {
        AllPerfectCheck();
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
        int intScore = (int)(score);
        text.text = intScore.ToString();
    }

    public void AddGood()
    {
        score += unitScore;
        int intScore = (int)(score);
        text.text = intScore.ToString();
    }

    private void AllPerfectCheck()
    {
        float acc = accCounter.acc;

        // 仮にMax4000コンボだとすると1コンボ辺りのscore = 250となる
        if (score >= 999750 && acc == 100.0f)
        {
            score = 1000000;
            text.text = score.ToString();
        }
    }

    public int GetScore()
    { 
        return (int)score;
    }

    private void NullCheck()
    {
        text.IsNull();
        timingJudgment.IsNull();
        accCounter.IsNull();
    }
}
