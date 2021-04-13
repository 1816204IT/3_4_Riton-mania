using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Acc（精度）計算クラス
/// </summary>
public class AccCounter : MonoBehaviour
{
    public float Acc { get; private set; } = 0.0f;
    public int TotalNoteNum { get; private set; } = 0;
    public int TotalPerfectNum { get; private set; } = 0;
    public int TotalGoodNum { get; private set; } = 0;
    public int TotalMissNum { get; private set; } = 0;
    
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
        TotalNoteNum++;
        TotalPerfectNum++;
        CalculateAcc();
    }
    
    public void AddGood()
    {
        TotalNoteNum++;
        TotalGoodNum++;
        CalculateAcc();
    }

    public void AddMiss()
    {
        TotalNoteNum++;
        TotalMissNum++;
        CalculateAcc();
    }

    private void CalculateAcc()
    {
        unitAcc = 100.0f / (float)TotalNoteNum;
        float accurateAcc = TotalPerfectNum * unitAcc + TotalGoodNum / 2.0f * unitAcc;
        Acc = accurateAcc;

        // 全てPerfectならAcc100%とする
        if ( (TotalMissNum == 0) && (TotalGoodNum == 0) )
        {
            Acc = 100.0f;
        }

        text.text = Acc.ToString("f2") + "%";
    }

    private void NullCheck()
    {
        text.IsNull();
    }
}
