using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteDataConverter : MonoBehaviour
{
    [SerializeField]
    private GameObject JudgmentBar = null;

    private MusicPlayer musicPlayer = null;
    private float basePos;          // ユーザーオフセットを考慮したタイミングバーの位置
    private float baseBeatSpanLen;  // LPB = 1/1 の時のY座標の間隔

    void Start()
    {
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();

        if (JudgmentBar == null || musicPlayer == null)
        {
            Debug.Log("nullを検知");
        }

        Init();
    }

    public void Init()
    {
        basePos = JudgmentBar.transform.position.y + UserPreference._instance._userOffset;
        baseBeatSpanLen = (musicPlayer._clapSpan * UserPreference._instance._notesSpeed);
    }

    //Y座標を曲の位置に変換する
    //public float ConvertTiming(float posY)
    //{
    //    float len = posY - basePos;
    //    float test = len / (musicPlayer._clapSpan * UserPreference._instance._notesSpeed);
    //    return musicPlayer.offsetedTime + musicPlayer._clapSpan * test;
    //}

    //曲の位置をbeatNumに変換する
    public int ConvertBeatNum(float time, int LPB)
    {
        float lenY = musicPlayer._clapSpan / LPB;
        return (int)(time / lenY);
    }

    //曲の再生位置に対応するノーツのY座標(0からの距離)を計算する
    public float ConvertDistance(int LPB, int num)
    {
        float timeIgnoredPos =  (baseBeatSpanLen / LPB) * num; // 曲再生時間0の時のY座標
        float timeLen = musicPlayer.offsetedTime * UserPreference._instance._notesSpeed; // 現在の曲の再生で進んだ距離
        return timeIgnoredPos - timeLen;
    }
}
