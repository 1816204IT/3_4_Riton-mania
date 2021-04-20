using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スコア計算クラス
/// 100%のプレイをするとスコアが丁度100万点となる。
/// PerfectはGoodの2倍のスコアとし、Missは加点しない。
/// 例　コンボ数100なら、Perfect = 1000000 / 100 = 10000点となり、Good = 5000点となる。
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

        // 0.1秒後にunitScoreを設定する
        Invoke("SetUnitScore", 0.1f);
    }

    private void Update()
    {
        AllPerfectCheck();
    }

    /// <summary>
    /// スコアの1単位(Good1個の加算値)を計算する
    /// </summary>
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

    /// <summary>
    /// Perfect判定のスコアを加算する
    /// </summary>
    public void AddPerfect()
    {
        score += unitScore * 2.0f;
        int intScore = (int)(score);
        text.text = intScore.ToString();
    }

    /// <summary>
    /// Good判定のスコアを加算する
    /// </summary>
    public void AddGood()
    {
        score += unitScore;
        int intScore = (int)(score);
        text.text = intScore.ToString();
    }

    /// <summary>
    /// Acc100%で曲をクリアした際にスコアを最大の100万点とする
    /// 計算誤差により100万点にならない場合のチェック処理
    /// </summary>
    private void AllPerfectCheck()
    {
        float acc = accCounter.Acc;

        // 仮にMax4000コンボだとすると1コンボ辺りのscore = 250となる
        if (score >= 999750 && acc == 100.0f)
        {
            score = 1000000;
            text.text = score.ToString();
        }
    }

    /// <summary>
    /// スコアを取得する
    /// </summary>
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
