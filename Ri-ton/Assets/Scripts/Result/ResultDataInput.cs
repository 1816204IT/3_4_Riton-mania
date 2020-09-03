using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct ResultShowData
{
    public int score;
    public float acc;
    public int maxCombo;
    public int combo;
    public int perfectNum;
    public int goodNum;
    public int missNum;
    public int rankImageNum;
}

// プレイ終了時のリザルトデータをプレイシーンから受け取るクラス
public class ResultDataInput : MonoBehaviour
{
    private ResultShowData data = new ResultShowData();

    [SerializeField]
    private Text scoreText = null;
    [SerializeField]
    private Text accText = null;
    [SerializeField]
    private Text comboText = null;
    [SerializeField]
    private Text perfectText = null;
    [SerializeField]
    private Text goodText = null;
    [SerializeField]
    private Text missText = null;
    [SerializeField]
    private Image rankImage = null;

    [SerializeField]
    private Text musicTitleText = null;
    [SerializeField]
    private Text difficultyText = null;
    [SerializeField]
    private Image characterImage = null;

    void Start()
    {
        if (scoreText == null || accText == null || comboText == null || perfectText == null
             || goodText == null || missText == null || characterImage == null || rankImage == null
             || difficultyText == null || musicTitleText == null)
        {
            Debug.Log("nullを検知");
        }
    
        scoreText.text = data.score.ToString("N0");
        accText.text = (data.acc / 100.0f).ToString();
        comboText.text = data.combo.ToString() + "/" + data.maxCombo.ToString();
        perfectText.text = data.perfectNum.ToString();
        goodText.text = data.goodNum.ToString();
        missText.text = data.missNum.ToString();
        rankImage.sprite = RankImageList._instance.GetSprite(data.rankImageNum);

        musicTitleText.text = SelectedMap._instance._musicName;
        difficultyText.text = SelectedMap._instance._difficultyName;
        characterImage.sprite = CharacterImageList._instance.GetSprite(UserPreference._instance._characterNum);
    }

    public void SetResultShowData(ResultShowData inData)
    {
        data = inData;
    }

    public int _score
    { 
        get { return data.score; }
    }
}
