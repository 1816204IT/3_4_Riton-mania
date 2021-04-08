using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Ritonmania;

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
    private float normalButtonHeight = 0.0f;
    [SerializeField]
    private float selectingButtonHeight = 0.0f;
    [SerializeField]
    private DifficultyButtons buttonImageObj = default;
    [SerializeField]
    private Image playButtonImage = null;
    [SerializeField]
    private DifficultyColor difficultyColor;

    private RectTransform[] buttonsRectTransform = new RectTransform[(int)DifficultyType.MAX];
    private Image[] buttonsImage = new Image[(int)DifficultyType.MAX];
    private Text[] buttonsText = new Text[(int)DifficultyType.MAX];
    private Image bigNodeBgImage = default;
    private BigNodeInformation bigNode = null;
    private MusicNodeInformation musicNodeInformation = null;

    void Awake()
    {
        bigNodeBgImage = GameObject.FindGameObjectWithTag("BigNode").GetComponent<Image>();
        musicNodeInformation = GameObject.FindGameObjectWithTag("MusicNodesRoot").GetComponent<MusicNodeInformation>();
        bigNode = GameObject.FindGameObjectWithTag("BigNode").GetComponent<BigNodeInformation>();


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
            int num = (int)SelectedMap.instance.nowDifficulty;
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
        string diffName = SelectedMap.instance.difficultyName;
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
        SelectedMap.instance.difficultyName = diffName;
        SelectedMap.instance.nowDifficulty = diffType;
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
        OnClickDifficultyButton("Easy", DifficultyType.EASY, difficultyColor.easy);
    }

    public void OnClickNormalButton()
    {
        OnClickDifficultyButton("Normal", DifficultyType.NORMAL, difficultyColor.normal);
    }

    public void OnClickHardButton()
    {
        OnClickDifficultyButton("Hard", DifficultyType.HARD, difficultyColor.hard);
    }

    public void OnClickExpertButton()
    {
        OnClickDifficultyButton("Expert", DifficultyType.EXPERT, difficultyColor.expert);
    }

    private void NullCheck()
    {
        buttonImageObj.Easy.IsNull();
        buttonImageObj.Normal.IsNull();
        buttonImageObj.Hard.IsNull();
        buttonImageObj.Expert.IsNull();
        bigNodeBgImage.IsNull();
        musicNodeInformation.IsNull();
        playButtonImage.IsNull();
    }
}