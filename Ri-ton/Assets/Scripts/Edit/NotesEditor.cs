using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using NoteEditor.DTO;

/// <summary>
/// Edit画面でノーツを配置するクラス(jsonファイルに書き込み)
/// </summary>
public class NotesEditor : MonoBehaviour
{
    private bool notesSetMode = false;

    private MusicPlayer musicPlayer = null;
    private GameObject mouseFollowNote = null;
    private TimingBar timingBar = null;
    private JsonManager jsonManager = null;
    private MouseFollow mouseFollow = null;
    private NoteDataConverter noteDataConverter = null;
    private PlayingNoteData playingNoteData = null;
    private int LPB = 1;
    private const int maxLPB = 8;

    [SerializeField]
    private GameObject judgmentBar = null;
    [SerializeField]
    private GameObject verticalLineLeft = null;
    [SerializeField]
    private GameObject verticalLinRight = null;
    [SerializeField]
    private Text beatSpanText = null;

    //設置するノーツデータ
    MusicDTO.Note clickDownNote = new MusicDTO.Note();

    void Start()
    {
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        mouseFollowNote = GameObject.FindGameObjectWithTag("MouseFollowNote");
        mouseFollow = mouseFollowNote.GetComponent<MouseFollow>();
        timingBar = GameObject.FindGameObjectWithTag("TimingBarManager").GetComponent<TimingBar>();
        jsonManager = GameObject.FindGameObjectWithTag("JsonManager").GetComponent<JsonManager>();
        noteDataConverter = GameObject.FindGameObjectWithTag("NoteDataConverter").GetComponent<NoteDataConverter>();
        playingNoteData = GameObject.FindGameObjectWithTag("PlayingNoteData").GetComponent<PlayingNoteData>();

        if (mouseFollowNote == null || mouseFollow == null || timingBar == null|| jsonManager == null
            || verticalLineLeft == null || verticalLinRight == null || beatSpanText == null 
            || musicPlayer == null || noteDataConverter == null || judgmentBar == null || playingNoteData == null)
        {
            Debug.Log("nullを検知");
        }

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

            //設置ノーツデータを作成
            clickDownNote.LPB = LPB;
            clickDownNote.num = noteDataConverter.ConvertBeatNum(time, LPB);
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
            clickUpNote.LPB = LPB;
            clickUpNote.num = noteDataConverter.ConvertBeatNum(time, LPB);
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
            clickDownNote.LPB = LPB;
            clickDownNote.num = noteDataConverter.ConvertBeatNum(time, LPB);
            clickDownNote.lane = GetLaneNum(mousePos.x);
            clickDownNote.type = 0;
            playingNoteData.RemoveNote(clickDownNote);
            playingNoteData.SaveNoteData();
        }
    }

    //クリックした際のマウス座標が有効かどうか
    private bool IsClickedPosValid(ref Vector3 mousePos, ref float time)
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

    //クリックしたX座標が第何レーンかを返す
    private int GetLaneNum(float posX)
    {
        posX += 300; // この300は何だろう
        return (int)(posX / UserPreference._instance._note_size_x);
    }

    //クリックしたY座標がaudioSource.timeの何秒に当たるかを返す
    private float GetAudioSourceTime(float mousePosY)
    {
        float len = mousePosY - judgmentBar.transform.position.y/* + UserPreference._instance._userOffset*/;
        len -= musicPlayer._offset * UserPreference._instance._notesSpeed;
        return musicPlayer._audioSource.time + (len / UserPreference._instance._notesSpeed);
    }

    public Vector3 GetSnappedPos(Vector3 pos)
    {
        float posX = UserPreference._instance._notePosXOfLaneZero + GetLaneNum(pos.x) * UserPreference._instance._note_size_x;
        float unit = musicPlayer._clapSpan * (UserPreference._instance._notesSpeed / LPB);
        float basePos = timingBar._barBasePosY;
        float num = basePos - pos.y;
        float len = num % unit;
        float posY = pos.y + len;

        return new Vector3(posX, posY, 0);
    }

    //配置モードON/OFF切り替え
    public void ToggleNotesSetMode()
    {
        notesSetMode = !notesSetMode;
        if (notesSetMode)
        {
            mouseFollowNote.SetActive(true);
        }
        else
        {
            mouseFollowNote.SetActive(false);
        }
    }

    //ビートスナップ間隔変更
    public void ChangeBeatSpanInterval()
    {
        if (LPB == 1)
        {
            LPB = 2;
            beatSpanText.text = "1/2";
        }
        else if (LPB == 2)
        {
            LPB = 3;
            beatSpanText.text = "1/3";
        }
        else if (LPB == 3)
        {
            LPB = 4;
            beatSpanText.text = "1/4";
        }
        else if (LPB == 4)
        {
            LPB = 6;
            beatSpanText.text = "1/6";
        }
        else if (LPB == 6)
        {
            LPB = 8;
            beatSpanText.text = "1/8";
        }
        else
        {
            LPB = 1;
            beatSpanText.text = "1/1";
        }
    }

    public int _LPB
    {
        get { return LPB; }
    }

    public int _maxLPB
    {
        get { return maxLPB; }
    }

    public bool _notesSetMode
    {
        get { return notesSetMode; }
    }
}
