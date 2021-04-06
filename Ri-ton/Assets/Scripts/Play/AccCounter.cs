using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Acc（精度）計算クラス
/// </summary>
public class AccCounter : MonoBehaviour
{
    public float acc { get; private set; } = 0.0f;
    public int totalNoteNum { get; private set; } = 0;
    public int totalPerfectNum { get; private set; } = 0;
    public int totalGoodNum { get; private set; } = 0;
    public int totalMissNum { get; private set; } = 0;
    
    private float unitAcc = 0.0f; // perfect判定を取った時に増加するAcc量
    private Text text = null;

    void Start()
    {
        text = this.GetComponent<Text>();
        NullCheck();
        text.text = "100.00%";
    }

    public void AddPerfect()
    {
        totalNoteNum++;
        totalPerfectNum++;
        CalculateAcc();
    }
    
    public void AddGood()
    {
        totalNoteNum++;
        totalGoodNum++;
        CalculateAcc();
    }

    public void AddMiss()
    {
        totalNoteNum++;
        totalMissNum++;
        CalculateAcc();
    }

    private void CalculateAcc()
    {
        unitAcc = 100.0f / (float)totalNoteNum;
        float accurateAcc = totalPerfectNum * unitAcc + totalGoodNum / 2.0f * unitAcc;
        acc = accurateAcc;

        // 全てPerfectならAcc100%とする
        if ( (totalMissNum == 0) && (totalGoodNum == 0) )
        {
            acc = 100.0f;
        }

        text.text = acc.ToString("f2") + "%";
    }

    private void NullCheck()
    {
        text.IsNull(nameof(text));
    }
}
