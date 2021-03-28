using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Editシーンにてタイミングバーを配置するクラス
/// </summary>
public class TimingBar : MonoBehaviour
{
    [SerializeField]
    private GameObject JudgmentBar = null;

    private MusicPlayer musicPlayer = null;
    private List<GameObject> whiteBars = new List<GameObject>();
    private List<GameObject> redBars = new List<GameObject>();
    private List<GameObject> blueBars = new List<GameObject>();
    private List<GameObject> purpleBars = new List<GameObject>();
    private List<GameObject> yellowBars = new List<GameObject>();
    private NotesEditor notesEditor = null;
    private bool isShowBar = true;
    private int whiteBarPutNum = 0;
    public float barBasePosY { get; private set; } = 0;

    void Start()
    {
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        notesEditor = GameObject.FindGameObjectWithTag("NotesEditor").GetComponent<NotesEditor>();

        FindColorBarObject(whiteBars, "TimingBarWhite");
        FindColorBarObject(redBars, "TimingBarRed");
        FindColorBarObject(blueBars, "TimingBarBlue");
        FindColorBarObject(purpleBars, "TimingBarPurple");
        FindColorBarObject(yellowBars, "TimingBarYellow");

        if (JudgmentBar == null || notesEditor == null
            || whiteBars.Count == 0 || redBars.Count == 0 || 
            blueBars.Count == 0 || purpleBars.Count == 0 || yellowBars.Count == 0 || musicPlayer == null)
        {
            Debug.Log("nullを検知");
        }
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
        float jPosY = JudgmentBar.transform.position.y + UserPreference.instance._userOffset;
        float length = (musicPlayer.offsetedTime % (musicPlayer._clapSpan * 4)) * UserPreference.instance._notesSpeed;

        //判定バーに最も近いタイミングバーを基点とする
        barBasePosY = jPosY - length;

        //カラーバーバーを配置していく
        SetBar(barBasePosY);
    }

    //1/1以下のカラーバーを配置を配置していく
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

    //バーを色分けして配置する
    private void DivideCaseLPB(ref float tmpPosY, ref int usedBarNumMain, ref int usedBarNumWhite, ref int usedBarNumRed, ref int usedBarNumBlue, ref int usedBarNumPurple, ref int usedBarNumYellow, bool isSetUpper)
    {
        for (int i = 0; i < notesEditor.LPB +1; i++)
        {
            if (i == 0)
            {
                continue;
            }

            // 1/1白線を配置
            if (i == notesEditor.LPB)
            {
                int num = 0;
                foreach (GameObject bar in whiteBars)
                {
                    num++;
                    if (num > usedBarNumWhite)
                    {
                        whiteBarPutNum++;
                        usedBarNumWhite++;
                        if ((whiteBarPutNum % 4) == 0)
                        {
                            // この場合は小節線(太い線)を別Scriptで配置するためskipする
                            float len = musicPlayer._clapSpan * (UserPreference.instance._notesSpeed / notesEditor.LPB);
                            tmpPosY = isSetUpper ? (tmpPosY + len) : (tmpPosY - len);
                        }
                        else
                        {
                            SetBarPosition(bar, ref tmpPosY, isSetUpper, 4);
                        }
                        break;
                    }
                }
                continue;
            }

            // 1/2赤線を配置
            bool isEvenLPB = ((notesEditor.LPB % 2) == 0); // LPBが偶数か(1/1や1/2ならtrue、1/3ならfalse)
            if (isEvenLPB && (i == notesEditor.LPB / 2))
            {
                int num = 0;
                foreach (GameObject bar in redBars)
                {
                    num++;
                    if (num > usedBarNumRed)
                    {
                        usedBarNumRed++;
                        SetBarPosition(bar, ref tmpPosY, isSetUpper, 4);
                        break;
                    }
                }
                continue;
            }

            // 1/3,1/6紫線を配置
            bool isPurple = ((notesEditor.LPB % 3) == 0);
            if (isPurple && (i == 1 || i == 2 || i == 4 || i == 5))
            {
                int num = 0;
                foreach (GameObject bar in purpleBars)
                {
                    num++;
                    if (num > usedBarNumPurple)
                    {
                        usedBarNumPurple++;
                        SetBarPosition(bar, ref tmpPosY, isSetUpper, 3);
                        break;
                    }
                }
                continue;
            }

            // 1/4青線を配置
            bool isBlue = ((notesEditor.LPB % 4) == 0);
            bool indexCheck = false;
            if (notesEditor.LPB == 4)
            {
                indexCheck = (i == 1 || i == 3 || i == 4 || i == 5);
            }
            if (notesEditor.LPB == 8)
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
                        SetBarPosition(bar, ref tmpPosY, isSetUpper, 4);
                        break;
                    }
                }
                continue;
            }

            //1/8黄色線を配置
            bool isYellow = ((notesEditor.LPB % 8) == 0);
            if (isYellow && (i % 2 != 0))
            {
                int num = 0;
                foreach (GameObject bar in yellowBars)
                {
                    num++;
                    if (num > usedBarNumYellow)
                    {
                        usedBarNumYellow++;
                        SetBarPosition(bar, ref tmpPosY, isSetUpper, 8);
                        break;
                    }
                }
                continue;
            }
        }
    }

    //全てのタイミングバーを画面外に移動(setActiveは重い処理のようなので座標移動で誤魔化す)
    private void BarMoveOutOfScreen(List<GameObject> bars)
    {
        foreach (GameObject bar in bars)
        {
            var tmpPos = bar.transform.position;
            bar.transform.position = new Vector3(tmpPos.x, 1600, tmpPos.z);
        }
    }

    //タイミングバーのY座標をセットしていく
    ///@param isSetUpper 上方向にセットしていくか
    private void SetBarPosition(GameObject bar, ref float tmpPosY, bool isSetUpper, int LPB)
    {
        float len = musicPlayer._clapSpan * (UserPreference.instance._notesSpeed / notesEditor.LPB);

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

    public void ToggleShowColorBar()
    {
        isShowBar = !isShowBar;
    }
}