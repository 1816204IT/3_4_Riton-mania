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
            int num = (int)SelectedMap._instance._nowDifficulty;
            num = (num + 1) % (int)DifficultyType.MAX;
            
            if (num == (int)DifficultyType.EASY)
            {
                OnclickEasyButton();
            }
            else if (num == (int)DifficultyType.NORMAL)
            {
                OnclickNormalButton();
            }
            else if (num == (int)DifficultyType.HARD)
            {
                OnclickHardButton();
            }
            else
            {
                OnclickExpertButton();
            }
        }
    }

    public void Initialize()
    {
        string diffName = SelectedMap._instance._difficultyName;
        if (diffName == "Easy")
        {
            OnclickEasyButton();
        }
        else if (diffName == "Normal")
        {
            OnclickNormalButton();
        }
        else if (diffName == "Hard")
        {
            OnclickHardButton();
        }
        else if (diffName == "Expert")
        {
            OnclickExpertButton();
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

    public void OnclickEasyButton()
    {
        SelectedMap._instance._difficultyName = "Easy";
        SelectedMap._instance._nowDifficulty = DifficultyType.EASY;
        AllButtonsHeightReset();
        Vector2 tmpSize = buttonsRectTransform[(int)DifficultyType.EASY].sizeDelta;
        buttonsRectTransform[(int)DifficultyType.EASY].sizeDelta = new Vector2(tmpSize.x, selectingButtonHeight);
        Color color = colorOfDifficulty.Easy;
        buttonsImage[(int)DifficultyType.EASY].color = color;
        buttonsText[(int)DifficultyType.EASY].color = Color.white;
        bigNodeBgImage.color = color;
        bigNodeBgImage.transform.Find("inLine").gameObject.GetComponent<Image>().color = color;
        playButtonImage.color = color;
        musicNodeInformation.UpdateInformationByChangeDifficulty();
    }

    public void OnclickNormalButton()
    {
        SelectedMap._instance._difficultyName = "Normal";
        SelectedMap._instance._nowDifficulty = DifficultyType.NORMAL;
        AllButtonsHeightReset();
        Vector2 tmpSize = buttonsRectTransform[(int)DifficultyType.NORMAL].sizeDelta;
        buttonsRectTransform[(int)DifficultyType.NORMAL].sizeDelta = new Vector2(tmpSize.x, selectingButtonHeight);
        Color color = colorOfDifficulty.Normal;
        buttonsImage[(int)DifficultyType.NORMAL].color = color;
        buttonsText[(int)DifficultyType.NORMAL].color = Color.white;
        bigNodeBgImage.color = color;
        bigNodeBgImage.transform.Find("inLine").gameObject.GetComponent<Image>().color = color;
        playButtonImage.color = color;
        musicNodeInformation.UpdateInformationByChangeDifficulty();
    }

    public void OnclickHardButton()
    {
        SelectedMap._instance._difficultyName = "Hard";
        SelectedMap._instance._nowDifficulty = DifficultyType.HARD;
        AllButtonsHeightReset();
        Vector2 tmpSize = buttonsRectTransform[(int)DifficultyType.HARD].sizeDelta;
        buttonsRectTransform[(int)DifficultyType.HARD].sizeDelta = new Vector2(tmpSize.x, selectingButtonHeight);
        Color color = colorOfDifficulty.Hard;
        buttonsImage[(int)DifficultyType.HARD].color = color;
        buttonsText[(int)DifficultyType.HARD].color = Color.white;
        bigNodeBgImage.color = color;
        bigNodeBgImage.transform.Find("inLine").gameObject.GetComponent<Image>().color = color;
        playButtonImage.color = color;
    }

    public void OnclickExpertButton()
    {
        SelectedMap._instance._difficultyName = "Expert";
        SelectedMap._instance._nowDifficulty = DifficultyType.EXPERT;
        AllButtonsHeightReset();
        Vector2 tmpSize = buttonsRectTransform[(int)DifficultyType.EXPERT].sizeDelta;
        buttonsRectTransform[(int)DifficultyType.EXPERT].sizeDelta = new Vector2(tmpSize.x, selectingButtonHeight);
        Color color = colorOfDifficulty.Expert;
        buttonsImage[(int)DifficultyType.EXPERT].color = color;
        buttonsText[(int)DifficultyType.EXPERT].color = Color.white;
        bigNodeBgImage.color = color;
        bigNodeBgImage.transform.Find("inLine").gameObject.GetComponent<Image>().color = color;
        playButtonImage.color = color;
        musicNodeInformation.UpdateInformationByChangeDifficulty();
    }
}
