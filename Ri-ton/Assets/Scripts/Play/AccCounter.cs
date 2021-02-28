using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Acc（精度）計算クラス
/// </summary>
public class AccCounter : MonoBehaviour
{
    private Text text = null;

    private float acc = 0.0f;
    private float unitAcc = 0.0f; // perfect判定を取った時に増加するAcc量
    private int totalNoteNum = 0;
    private int totalPerfectNum = 0;
    private int totalGoodNum = 0;
    private int totalMissNum = 0;

    void Start()
    {
        text = this.GetComponent<Text>();

        if (text == null)
        {
            Debug.Log("nullを検知");
        }
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

    public int _totalNoteNum
    { 
        get { return totalNoteNum; }
    }

    public int _totalPerfectNum
    {
        get { return totalPerfectNum; }
    }

    public int _totalGoodNum
    {
        get { return totalGoodNum; }
    }

    public int _totalMissNum
    {
        get { return totalMissNum; }
    }

    public float _acc
    {
        get { return acc; }
    }
}
