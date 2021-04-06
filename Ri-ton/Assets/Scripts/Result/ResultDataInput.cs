using UnityEngine;
using UnityEngine.UI;
using Ritonmania;

namespace Ritonmania
{
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
        public bool isAciveHighScore;
    }
}

/// <summary>
/// プレイ終了時のリザルトデータをプレイシーンから受け取るクラス
/// </summary>
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
    private Text newRecordText = null;

    [SerializeField]
    private Text musicTitleText = null;
    [SerializeField]
    private Text difficultyText = null;
    [SerializeField]
    private Image characterImage = null;

    void Start()
    {
        NullCheck();
 
        scoreText.text = data.score.ToString("N0");
        accText.text = (data.acc / 100.0f).ToString();
        comboText.text = data.combo.ToString() + "/" + data.maxCombo.ToString();
        perfectText.text = data.perfectNum.ToString();
        goodText.text = data.goodNum.ToString();
        missText.text = data.missNum.ToString();
        rankImage.sprite = RankImageList.instance.GetSprite(data.rankImageNum);
        if (data.isAciveHighScore)
        {
            newRecordText.text = "new record!!";
        }
        else
        {
            newRecordText.text = "";
        }

        musicTitleText.text = SelectedMap.instance.musicName;
        difficultyText.text = SelectedMap.instance.difficultyName;
        characterImage.sprite = CharacterInfoList.instance.GetSprite(UserPreference.instance.GetCharacterNumber());
        characterImage.sprite = CharacterInfoList.instance.GetSprite(UserPreference.instance.GetCharacterNumber());

        // -----最高成績なら文字色を黄色にする-----
        if ((data.acc / 100.0f) == 100.0f)
        {
            accText.color = Color.yellow;
        }
        if (data.combo == data.maxCombo)
        {
            comboText.color = Color.yellow;
        }
        if (data.perfectNum == data.maxCombo)
        {
            perfectText.color = Color.yellow;
        }
        if (data.goodNum == 0)
        {
            goodText.color = Color.yellow;
        }
        if (data.missNum == 0)
        {
            missText.color = Color.yellow;
        }
    }

    public void SetResultShowData(ResultShowData inData)
    {
        data = inData;
    }

    public int GetScore()
    { 
        return data.score;
    }

    private void NullCheck()
    {
        scoreText.IsNull(nameof(scoreText));
        accText.IsNull(nameof(accText));
        comboText.IsNull(nameof(comboText));
        perfectText.IsNull(nameof(perfectText));
        goodText.IsNull(nameof(goodText));
        missText.IsNull(nameof(missText));
        characterImage.IsNull(nameof(characterImage));
        rankImage.IsNull(nameof(rankImage));
        difficultyText.IsNull(nameof(difficultyText));
        musicTitleText.IsNull(nameof(musicTitleText));
        newRecordText.IsNull(nameof(newRecordText));
    }
}