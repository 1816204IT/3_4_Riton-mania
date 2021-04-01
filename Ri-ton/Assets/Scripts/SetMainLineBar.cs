using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 小節線バー(太いバー)を配置する
/// </summary>
public class SetMainLineBar : MonoBehaviour
{
    [SerializeField]
    private GameObject baseTimingBar = null;
    [SerializeField]
    private GameObject JudgmentBar = null;

    private MusicPlayer musicPlayer = null;
    private List<GameObject> mainBars = new List<GameObject>();

    void Start()
    {
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();

        GameObject[] array = GameObject.FindGameObjectsWithTag("TimingBarMain");
        foreach (GameObject bar in array)
        {
            mainBars.Add(bar);
        }

        NullCheck();
    }

    void Update()
    {
        //全てのタイミングバーを画面外に移動(setActiveは重い処理のようなので座標移動で誤魔化す)
        var tmpPos = baseTimingBar.transform.position;
        baseTimingBar.transform.position = new Vector3(tmpPos.x, 4000, tmpPos.z);
        foreach (GameObject bar in mainBars)
        {
            var oldPos = bar.transform.position;
            bar.transform.position = new Vector3(oldPos.x, 4000, oldPos.z);
        }

        float jPosY = JudgmentBar.transform.position.y + UserPreference.instance.UserOffset();
        float length = (musicPlayer.offsetedTime % (musicPlayer.ClapSpan() * 4)) * UserPreference.instance.NoteSpeed();

        //判定バーに最も近いタイミングバーを基点とする
        Vector3 tPos = baseTimingBar.transform.position;
        Vector3 basePos = new Vector3(tPos.x, jPosY - length, tPos.z);
        baseTimingBar.transform.position = basePos;

        SetMianLineBar(basePos.y);
    }

    void SetMianLineBar(float basePosY)
    {
        float len = musicPlayer.ClapSpan() * 4 * UserPreference.instance.NoteSpeed();
        float tmpPosY = basePosY;
        int usedMainBarNum = 0;

        //基準のY座標より下方向のバーを配置
        tmpPosY -= len;
        while (tmpPosY > 0)
        {
            int num = 0;
            foreach (GameObject bar in mainBars)
            {
                num++;
                if (num > usedMainBarNum)
                {
                    usedMainBarNum++;
                    var pos = bar.transform.position;
                    bar.transform.position = new Vector3(pos.x, tmpPosY, pos.z);
                    break;
                }
            }
            tmpPosY = tmpPosY - len;
        }

        tmpPosY = basePosY;
        tmpPosY += len;
        //基準のY座標より上方向のバーを配置
        while (tmpPosY < 3100)
        {
            int num = 0;
            foreach (GameObject bar in mainBars)
            {
                num++;
                if (num > usedMainBarNum)
                {
                    usedMainBarNum++;
                    var pos = bar.transform.position;
                    bar.transform.position = new Vector3(pos.x, tmpPosY, pos.z);
                    break;
                }
            }
            tmpPosY = tmpPosY + len;
        }
    }

    private void NullCheck()
    {
        musicPlayer.IsNull(nameof(musicPlayer));
        JudgmentBar.IsNull(nameof(JudgmentBar));
        baseTimingBar.IsNull(nameof(baseTimingBar));

        if (mainBars.Count == 0)
        {
            Debug.Log("mainBars is Null");
        }
    }
}