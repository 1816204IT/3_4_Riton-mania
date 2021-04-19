using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Editシーンにてタイミングバーを配置するクラス
/// </summary>
public class TimingBar : MonoBehaviour
{
    public float BarBasePosY { get; private set; } = 0;

    [SerializeField]
    private GameObject judgmentBar = null;

    private MusicPlayer musicPlayer = null;
    private List<GameObject> whiteBars = new List<GameObject>();
    private List<GameObject> redBars = new List<GameObject>();
    private List<GameObject> blueBars = new List<GameObject>();
    private List<GameObject> purpleBars = new List<GameObject>();
    private List<GameObject> yellowBars = new List<GameObject>();
    private NoteEdit noteEditor = null;
    private bool isShowBar = true;
    private int whiteBarPutNum = 0;

    void Start()
    {
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        noteEditor = GameObject.FindGameObjectWithTag("NoteEditor").GetComponent<NoteEdit>();

        FindColorBarObject(whiteBars, "TimingBarWhite");
        FindColorBarObject(redBars, "TimingBarRed");
        FindColorBarObject(blueBars, "TimingBarBlue");
        FindColorBarObject(purpleBars, "TimingBarPurple");
        FindColorBarObject(yellowBars, "TimingBarYellow");

        NullCheck();
    }

    private void FindColorBarObject(in List<GameObject> barList, string tagName)
    {
        GameObject[] array = GameObject.FindGameObjectsWithTag(tagName);
        foreach (GameObject bar in array)
        {
            barList.Add(bar);
        }
    }

    void Update()
    {
        float jPosY = judgmentBar.transform.position.y + UserPreference.Instance.UserOffset();
        float length = (musicPlayer.offsetedTime % (musicPlayer.ClapSpan() * 4)) * UserPreference.Instance.NoteSpeed();

        // 判定バーに最も近いタイミングバーを基点とする
        BarBasePosY = jPosY - length;

        // カラーバーバーを配置していく
        SetBar(BarBasePosY);
    }

    /// <summary>
    /// 1/1以下のカラーバーを配置を配置していく
    /// </summary>
    /// <param name="basePosY">判定バーに最も近いタイミングバー</param>
    private void SetBar(float basePosY)
    {
        float tmpPosY = basePosY;

        //全てのタイミングバーを画面外に移動
        BarMoveOutOfScreen(whiteBars);
        BarMoveOutOfScreen(redBars);
        BarMoveOutOfScreen(blueBars);
        BarMoveOutOfScreen(purpleBars);
        BarMoveOutOfScreen(yellowBars);

        int usedBarNumMain = 0;
        int usedBarNumWhite = 0;
        int usedBarNumRed = 0;
        int usedBarNumBlue = 0;
        int usedBarNumPurple = 0;
        int usedBarNumYellow = 0;

        //基準のY座標より下方向のバーを配置
        whiteBarPutNum = 0;
        while (tmpPosY > 0)
        {
            DivideCaseLPB(ref tmpPosY, ref usedBarNumMain, ref usedBarNumWhite, ref usedBarNumRed, ref usedBarNumBlue, ref usedBarNumPurple, ref usedBarNumYellow, false);
        }

        tmpPosY = basePosY;

        //基準のY座標より上方向のバーを配置
        whiteBarPutNum = 0;
        while (tmpPosY < 1500)
        {
            DivideCaseLPB(ref tmpPosY, ref usedBarNumMain, ref usedBarNumWhite, ref usedBarNumRed, ref usedBarNumBlue, ref usedBarNumPurple, ref usedBarNumYellow, true);
        } 
    }

    /// <summary>
    /// バーを色分けして配置する
    /// </summary>
    private void DivideCaseLPB(ref float tempPosY, ref int usedBarNumMain, ref int usedBarNumWhite, ref int usedBarNumRed, ref int usedBarNumBlue, ref int usedBarNumPurple, ref int usedBarNumYellow, bool isSetUpper)
    {
        for (int i = 0; i < noteEditor.Lpb +1; i++)
        {
            if (i == 0)
            {
                continue;
            }

            // 1/1白線を配置
            if (i == noteEditor.Lpb)
            {
                int num = 0;
                foreach (GameObject bar in whiteBars)
                {
                    num++;
                    if (num > usedBarNumWhite)
                    {
                        whiteBarPutNum++;
                        usedBarNumWhite++;
                        SetBarPosition(bar, ref tempPosY, isSetUpper, 4);
                        break;
                    }
                }
                continue;
            }

            // 1/2赤線を配置
            bool isEvenLPB = ((noteEditor.Lpb % 2) == 0); // LPBが偶数か(1/1や1/2ならtrue、1/3ならfalse)
            if (isEvenLPB && (i == noteEditor.Lpb / 2))
            {
                int num = 0;
                foreach (GameObject bar in redBars)
                {
                    num++;
                    if (num > usedBarNumRed)
                    {
                        usedBarNumRed++;
                        SetBarPosition(bar, ref tempPosY, isSetUpper, 4);
                        break;
                    }
                }
                continue;
            }

            // 1/3,1/6紫線を配置
            bool isPurple = ((noteEditor.Lpb % 3) == 0);
            if (isPurple && (i == 1 || i == 2 || i == 4 || i == 5))
            {
                int num = 0;
                foreach (GameObject bar in purpleBars)
                {
                    num++;
                    if (num > usedBarNumPurple)
                    {
                        usedBarNumPurple++;
                        SetBarPosition(bar, ref tempPosY, isSetUpper, 3);
                        break;
                    }
                }
                continue;
            }

            // 1/4青線を配置
            bool isBlue = ((noteEditor.Lpb % 4) == 0);
            bool indexCheck = false;
            if (noteEditor.Lpb == 4)
            {
                indexCheck = (i == 1 || i == 3 || i == 4 || i == 5);
            }
            if (noteEditor.Lpb == 8)
            {
                indexCheck = (i == 2 || i == 6);
            }
            if (isBlue && indexCheck)
            {
                int num = 0;
                foreach (GameObject bar in blueBars)
                {
                    num++;
                    if (num > usedBarNumBlue)
                    {
                        usedBarNumBlue++;
                        SetBarPosition(bar, ref tempPosY, isSetUpper, 4);
                        break;
                    }
                }
                continue;
            }

            //1/8黄色線を配置
            bool isYellow = ((noteEditor.Lpb % 8) == 0);
            if (isYellow && (i % 2 != 0))
            {
                int num = 0;
                foreach (GameObject bar in yellowBars)
                {
                    num++;
                    if (num > usedBarNumYellow)
                    {
                        usedBarNumYellow++;
                        SetBarPosition(bar, ref tempPosY, isSetUpper, 8);
                        break;
                    }
                }
                continue;
            }
        }
    }

    /// <summary>
    /// 全てのタイミングバーを画面外に移動
    /// </summary>
    /// <param name="bars"></param>
    private void BarMoveOutOfScreen(List<GameObject> bars)
    {
        foreach (GameObject bar in bars)
        {
            var tmpPos = bar.transform.position;
            bar.transform.position = new Vector3(tmpPos.x, 1600, tmpPos.z);
        }
    }

    /// <summary>
    /// タイミングバーのY座標をセットしていく
    /// </summary>
    private void SetBarPosition(GameObject bar, ref float tmpPosY, bool isSetUpper, int LPB)
    {
        float len = musicPlayer.ClapSpan() * (UserPreference.Instance.NoteSpeed() / noteEditor.Lpb);

        tmpPosY = isSetUpper ? (tmpPosY + len) : (tmpPosY - len);
        if ((tmpPosY > 0) && (tmpPosY < 1500))
        {
            if (isShowBar)
            {
                var pos = bar.transform.position;
                bar.transform.position = new Vector3(pos.x, tmpPosY, pos.z);
            }
        }
    }

    /// <summary>
    /// バーの表示非表示切替
    /// </summary>
    public void ToggleShowColorBar()
    {
        isShowBar = !isShowBar;
    }

    private void NullCheck()
    {
        judgmentBar.IsNull();
        noteEditor.IsNull();
        musicPlayer.IsNull();
        if (whiteBars.Count == 0)
        {
            Debug.LogError("whiteBars.Count is Null");
        }
        if (redBars.Count == 0)
        {
            Debug.LogError("redBars is Null");
        }
        if (blueBars.Count == 0)
        {
            Debug.LogError("blueBars.Count == 0 is Null");
        }
        if (purpleBars.Count == 0)
        {
            Debug.LogError("purpleBars is Null");
        }
        if (yellowBars.Count == 0)
        {
            Debug.LogError("yellowBars is Null");
        }
    }
}