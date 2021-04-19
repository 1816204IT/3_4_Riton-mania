using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NoteEditor.DTO;

/// <summary>
/// Edit画面でノーツを配置するクラス(jsonファイルに書き込み)
/// </summary>
public class NoteEdit : MonoBehaviour
{
    public bool NoteSetMode { get; private set; }
    public int Lpb { get; private set; } = 1;

    [SerializeField]
    private GameObject judgmentBar = null;
    [SerializeField]
    private GameObject verticalLineLeft = null;
    [SerializeField]
    private GameObject verticalLinRight = null;
    [SerializeField]
    private Text beatSpanText = null;

    private MusicPlayer musicPlayer = null;
    private GameObject mouseFollowNote = null;
    private TimingBar timingBar = null;
    private JsonManager jsonManager = null;
    private MouseFollow mouseFollow = null;
    private NoteDataConverter noteDataConverter = null;
    private PlayingNoteData playingNoteData = null;
    private MusicDTO.Note clickDownNote = new MusicDTO.Note();  //設置するノーツデータ

    void Start()
    {
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        mouseFollowNote = GameObject.FindGameObjectWithTag("MouseFollowNote");
        mouseFollow = mouseFollowNote.GetComponent<MouseFollow>();
        timingBar = GameObject.FindGameObjectWithTag("TimingBarManager").GetComponent<TimingBar>();
        jsonManager = GameObject.FindGameObjectWithTag("JsonManager").GetComponent<JsonManager>();
        noteDataConverter = GameObject.FindGameObjectWithTag("NoteDataConverter").GetComponent<NoteDataConverter>();
        playingNoteData = GameObject.FindGameObjectWithTag("PlayingNoteData").GetComponent<PlayingNoteData>();
        NullCheck();

        mouseFollowNote.SetActive(false);
        clickDownNote.endNote = new List<MusicDTO.Note>();
    }

    void Update()
    {
        if (mouseFollowNote.activeSelf == false)
        {
            return;
        }

        //マウス左クリック押下でノーツ書き込み開始（左クリックを離した際に確定）
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = new Vector3();
            float time = 0;
            if (IsClickedPosValid(ref mousePos, ref time) == false)
            {
                //クリック押下時にノーツ書き込み範囲外ならエラー値を入れる
                clickDownNote.num = int.MinValue;
                return;
            }

            var debugPos = mouseFollow.GetMouseFollowNotePos();

            //設置ノーツデータを作成
            clickDownNote.LPB = Lpb;
            clickDownNote.num = noteDataConverter.ConvertBeatNum(time, Lpb);
            clickDownNote.lane = GetLaneNum(mousePos.x);
            clickDownNote.isJudgment = false;
        }

        //マウス左クリックを離した時にノーツを書き込む
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 mousePos = new Vector3();
            float time = 0;

            if (IsClickedPosValid(ref mousePos, ref time) == false)
            {
                return;
            }

            //設置ノーツデータを作成
            MusicDTO.Note clickUpNote = new MusicDTO.Note();
            clickUpNote.endNote = new List<MusicDTO.Note>();
            clickUpNote.LPB = Lpb;
            clickUpNote.num = noteDataConverter.ConvertBeatNum(time, Lpb);
            clickUpNote.lane = GetLaneNum(mousePos.x);
            clickDownNote.isJudgment = false;

            //クリック開始位置と終了位置が同じ場合
            if ((clickDownNote.lane == clickUpNote.lane) && (clickDownNote.num == clickUpNote.num))
            {
                clickUpNote.type = 0;
                clickUpNote.isLongNote = false;
                playingNoteData.AddNote(clickUpNote);
                return;
            }

            //クリック開始時のレーンとクリック終了時のレーンが違う場合
            //又はノーツ設置範囲外でクリック開始してノーツ設置範囲内でクリック終了した場合
            //クリック終了時の位置の単推しノーツとしてデータを書き込む
            if ((clickDownNote.lane != clickUpNote.lane) || clickDownNote.num == int.MinValue)
            {
                clickUpNote.type = 0;
                clickUpNote.isLongNote = false;
                playingNoteData.AddNote(clickUpNote);
                return;
            }

            //上から下にロングノーツをドラッグした場合
            MusicDTO.Note tmpNote = new MusicDTO.Note();
            float a = clickDownNote.num / (float)clickDownNote.LPB;
            float b = clickUpNote.num / (float)clickUpNote.LPB;
            if (a > b)
            {
                tmpNote = clickDownNote;
                clickDownNote = clickUpNote;
                clickUpNote = tmpNote;
            }

            //ロングノーツの終点ノーツ
            MusicDTO.Note endNote = new MusicDTO.Note();
            endNote.LPB = clickUpNote.LPB;
            endNote.num = clickUpNote.num;
            endNote.lane = clickUpNote.lane;
            endNote.isJudgment = false;
            endNote.isLongNote = true;
            endNote.type = 2;//2はロングノーツの終点
            //ロングノーツの始点ノーツ
            MusicDTO.Note longNote = new MusicDTO.Note();
            longNote.LPB = clickDownNote.LPB;
            longNote.num = clickDownNote.num;
            longNote.lane = clickDownNote.lane;
            longNote.isJudgment = false;
            longNote.isLongNote = true;
            longNote.type = 1;//1はロングノーツの始点
            longNote.endNote = new List<MusicDTO.Note>();
            longNote.endNote.Add(endNote);

            playingNoteData.AddNote(longNote);
        }

        //マウス右クリックでノーツ削除
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = new Vector3();
            float time = 0;

            if (IsClickedPosValid(ref mousePos, ref time) == false)
            {
                return;
            }

            //設置ノーツデータを作成
            clickDownNote.LPB = Lpb;
            clickDownNote.num = noteDataConverter.ConvertBeatNum(time, Lpb);
            clickDownNote.lane = GetLaneNum(mousePos.x);
            clickDownNote.type = 0;
            playingNoteData.RemoveNote(clickDownNote);
            playingNoteData.SaveNoteData();
        }
    }

    /// <summary>
    /// クリックした際のマウス座標が有効かどうかを返す
    /// </summary>
    /// <param name="mousePos">マウス座標</param>
    /// <param name="time">現在の曲の再生時間</param>
    /// <returns></returns>
    public bool IsClickedPosValid(ref Vector3 mousePos, ref float time)
    {
        if (mouseFollow.IsMousePosValid() == false)
        {
            return false;
        }

        mousePos = mouseFollow.GetMouseFollowNotePos();
        time = GetAudioSourceTime(mousePos.y);

        if (time < 0)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// クリックした座標が第何レーンかを返す
    /// </summary>
    /// <param name="posX">クリックしたX座標</param>
    private int GetLaneNum(float posX)
    {
        posX += 300;
        return (int)(posX / UserPreference.Instance.NoteSizeX());
    }

    /// <summary>
    /// クリックしたY座標がaudioSource.timeの何秒に当たるかを返す
    /// </summary>
    /// <param name="mousePosY">クリックしたY座標</param>
    private float GetAudioSourceTime(float mousePosY)
    {
        float len = mousePosY - judgmentBar.transform.position.y;
        len -= musicPlayer.Offset * UserPreference.Instance.NoteSpeed();
        return musicPlayer.AudioSource.time + (len / UserPreference.Instance.NoteSpeed());
    }

    /// <summary>
    /// 小節線にスナップした座標を返す
    /// </summary>
    /// <param name="pos">クリックした座標</param>
    public Vector3 GetSnappedPos(Vector3 pos)
    {
        float posX = UserPreference.Instance.NotePosXOfLaneZero() + GetLaneNum(pos.x) * UserPreference.Instance.NoteSizeX()  ;
        float unit = musicPlayer.ClapSpan() * (UserPreference.Instance.NoteSpeed() / Lpb);
        float basePos = timingBar.BarBasePosY;
        float num = basePos - pos.y;
        float len = num % unit;
        float posY = pos.y + len;

        return new Vector3(posX, posY, 0);
    }

    /// <summary>
    /// 配置モードON/Off切替
    /// </summary>
    public void ToggleNoteSetMode()
    {
        NoteSetMode = !NoteSetMode;
        if (NoteSetMode)
        {
            mouseFollowNote.SetActive(true);
        }
        else
        {
            mouseFollowNote.SetActive(false);
        }
    }

    /// <summary>
    /// ビートスナップ間隔変更
    /// </summary>
    public void ChangeBeatSpanInterval()
    {
        if (Lpb == 1)
        {
            Lpb = 2;
            beatSpanText.text = "1/2";
        }
        else if (Lpb == 2)
        {
            Lpb = 3;
            beatSpanText.text = "1/3";
        }
        else if (Lpb == 3)
        {
            Lpb = 4;
            beatSpanText.text = "1/4";
        }
        else if (Lpb == 4)
        {
            Lpb = 6;
            beatSpanText.text = "1/6";
        }
        else if (Lpb == 6)
        {
            Lpb = 8;
            beatSpanText.text = "1/8";
        }
        else
        {
            Lpb = 1;
            beatSpanText.text = "1/1";
        }
    }

    private void NullCheck()
    {
        mouseFollowNote.IsNull();
        mouseFollow.IsNull();
        timingBar.IsNull();
        jsonManager.IsNull();
        verticalLineLeft.IsNull();
        verticalLinRight.IsNull();
        beatSpanText.IsNull();
        musicPlayer.IsNull();
        noteDataConverter.IsNull();
        judgmentBar.IsNull();
        playingNoteData.IsNull();
    }
}