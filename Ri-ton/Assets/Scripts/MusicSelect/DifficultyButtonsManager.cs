using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 難易度ボタン管理クラス
/// </summary>
public class DifficultyButtonsManager : MonoBehaviour
{
    [System.Serializable]
    private struct DifficultyButtons
    { 
        public GameObject Easy;
        public GameObject Normal;
        public GameObject Hard;
        public GameObject Expert;
    }

    [SerializeField]
    private ScoreView scoreView = null;
    [SerializeField]
    private float normalButtonHeight = 40;
    [SerializeField]
    private float selectingButtonHeight = 70;

    [SerializeField]
    private DifficultyButtons buttonImageObj = default;
    private RectTransform[] buttonsRectTransform = new RectTransform[(int)DifficultyType.MAX];
    private Image[] buttonsImage = new Image[(int)DifficultyType.MAX];
    private Text[] buttonsText = new Text[(int)DifficultyType.MAX];

    private Image bigNodeBgImage = default;
    private BigNodeInformation bigNode = null;
    private MusicNodeInformation musicNodeInformation = null;

    [SerializeField]
    private Image playButtonImage = null;

    [SerializeField]
    private ColorOfDifficulty colorOfDifficulty;

    void Awake()
    {
        bigNodeBgImage = GameObject.FindGameObjectWithTag("BigNode").GetComponent<Image>();
        musicNodeInformation = GameObject.FindGameObjectWithTag("MusicNodesRoot").GetComponent<MusicNodeInformation>();
        bigNode = GameObject.FindGameObjectWithTag("BigNode").GetComponent<BigNodeInformation>();

        if (buttonImageObj.Easy == null || buttonImageObj.Normal == null || buttonImageObj.Hard == null
            || buttonImageObj.Expert == null || bigNodeBgImage == null || musicNodeInformation == null
            || playButtonImage == null)
        {
            Debug.Log("nullを検知");
        }
        if (SceneManager.GetActiveScene().name == "PlaySongSelect")
        {
            if (scoreView == null)
            {
                Debug.Log("nullを検知");
            }
        }

        buttonsRectTransform[(int)DifficultyType.EASY] = buttonImageObj.Easy.GetComponent<RectTransform>();
        buttonsRectTransform[(int)DifficultyType.NORMAL] = buttonImageObj.Normal.GetComponent<RectTransform>();
        buttonsRectTransform[(int)DifficultyType.HARD] = buttonImageObj.Hard.GetComponent<RectTransform>();
        buttonsRectTransform[(int)DifficultyType.EXPERT] = buttonImageObj.Expert.GetComponent<RectTransform>();

        buttonsImage[(int)DifficultyType.EASY] = buttonImageObj.Easy.GetComponent<Image>();
        buttonsImage[(int)DifficultyType.NORMAL] = buttonImageObj.Normal.GetComponent<Image>();
        buttonsImage[(int)DifficultyType.HARD] = buttonImageObj.Hard.GetComponent<Image>();
        buttonsImage[(int)DifficultyType.EXPERT] = buttonImageObj.Expert.GetComponent<Image>();

        buttonsText[(int)DifficultyType.EASY] = buttonImageObj.Easy.transform.Find("Text").GetComponent<Text>();
        buttonsText[(int)DifficultyType.NORMAL] = buttonImageObj.Normal.transform.Find("Text").GetComponent<Text>();
        buttonsText[(int)DifficultyType.HARD] = buttonImageObj.Hard.transform.Find("Text").GetComponent<Text>();
        buttonsText[(int)DifficultyType.EXPERT] = buttonImageObj.Expert.transform.Find("Text").GetComponent<Text>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            int num = (int)SelectedMap.instance._nowDifficulty;
            num = (num + 1) % (int)DifficultyType.MAX;
            
            if (num == (int)DifficultyType.EASY)
            {
                OnClickEasyButton();
            }
            else if (num == (int)DifficultyType.NORMAL)
            {
                OnClickNormalButton();
            }
            else if (num == (int)DifficultyType.HARD)
            {
                OnClickHardButton();
            }
            else
            {
                OnClickExpertButton();
            }
        }
    }

    public void Initialize()
    {
        string diffName = SelectedMap.instance._difficultyName;
        if (diffName == "Easy")
        {
            OnClickEasyButton();
        }
        else if (diffName == "Normal")
        {
            OnClickNormalButton();
        }
        else if (diffName == "Hard")
        {
            OnClickHardButton();
        }
        else if (diffName == "Expert")
        {
            OnClickExpertButton();
        }
        else
        {
            Debug.Log("無効な難易度名です");
        }
    }

    private void AllButtonsHeightReset()
    {
        foreach (RectTransform rt in buttonsRectTransform)
        {
            Vector2 tmpSize = rt.sizeDelta;
            rt.sizeDelta = new Vector2(tmpSize.x, normalButtonHeight);
        }
        //ランキング更新
        if (SceneManager.GetActiveScene().name == "PlaySongSelect")
        {
            scoreView.UpdateResultData();
        }
        //大画面情報の更新
        bigNode.InformationUpdate();
        //ボタンの色を灰色にする
        foreach (Image img in buttonsImage)
        {
            img.color = new Color(0.84f, 0.84f, 0.84f);
        }
        //ボタンの文字を灰色にする
        foreach (Text text in buttonsText)
        {
            text.color = new Color(0.84f, 0.84f, 0.84f);
        }
    }

    private void OnClickDifficultyButton(string diffName, DifficultyType diffType, Color diffColor)
    {
        SelectedMap.instance._difficultyName = diffName;
        SelectedMap.instance._nowDifficulty = diffType;
        AllButtonsHeightReset();
        Vector2 tmpSize = buttonsRectTransform[(int)diffType].sizeDelta;
        buttonsRectTransform[(int)diffType].sizeDelta = new Vector2(tmpSize.x, selectingButtonHeight);
        buttonsImage[(int)diffType].color = diffColor;
        buttonsText[(int)diffType].color = Color.white;
        bigNodeBgImage.color = diffColor;
        bigNodeBgImage.transform.Find("inLine").gameObject.GetComponent<Image>().color = diffColor;
        playButtonImage.color = diffColor;
        musicNodeInformation.UpdateInformationByChangeDifficulty();
    }

    public void OnClickEasyButton()
    {
        OnClickDifficultyButton("Easy", DifficultyType.EASY, colorOfDifficulty.easy);
    }

    public void OnClickNormalButton()
    {
        OnClickDifficultyButton("Normal", DifficultyType.NORMAL, colorOfDifficulty.normal);
    }

    public void OnClickHardButton()
    {
        OnClickDifficultyButton("Hard", DifficultyType.HARD, colorOfDifficulty.hard);
    }

    public void OnClickExpertButton()
    {
        OnClickDifficultyButton("Expert", DifficultyType.EXPERT, colorOfDifficulty.expert);
    }
}