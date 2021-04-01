using NoteEditor.DTO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤがノーツを叩いた際に判定を行う
/// 判定が終了したノーツをPlayingNoteDataから削除する
/// </summary>
public class TimingJudgment : MonoBehaviour
{
    // SerializeField関連
    [SerializeField]
    private int laneNum = 0;
    [SerializeField]
    private Text timingText = null;
    [SerializeField]
    private KeyEffect keyEffect = null;
    [SerializeField]
    private Text fastSlowText = null;
    [SerializeField]
    private AccCounter accCounter = null;
    [SerializeField]
    private JudgmentText judgmentText = null;
    [SerializeField]
    private ComboCounter comboCounter = null;
    [SerializeField]
    private ScoreCounter scoreCounter = null;

    // Find関連
    private JsonManager jsonManager = null;
    private MusicPlayer musicPlayer = null;
    private NoteSetter noteSetter = null;
    private AudioSource audioSource = null;
    private PlayingNoteData playingNoteData = null;
    private NoteDataConverter noteDataConverter = null;

    // 判定フレーム数
    private const int missFrame = 14;
    private const int goodFrame = 10;
    private const int perfectFrame = 6;
    private float missLen = 0.0f;
    private float goodLen = 0.0f;
    private float perfectLen = 0.0f;

    // ロングノーツ関連
    private bool isNowLongNote = false;         // ロングノーツの始点から終点の間のみtureとする
    private bool isHoldValid = false;           // 長押しの際にロングノーツとの判定を取るか(連続したロングノーツや、ロングノーツの始点前からホールドしていた際のチェック用)
    private float holdUpedCheatTime = 0.0f;     // ホールドの最後は判定を緩くする。ホールドを離した後はgood判定分のチートタイムを設ける
    private MusicDTO.Note judgmentingLongNote;  // 判定中のロングノーツ

    private MusicDTO.MapData mapData = new MusicDTO.MapData();
    float secondDistance = 0.0f; // 1秒間で進む距離

    void Start()
    {
        FindObjects();
        NullCheck();

        mapData = jsonManager.LoadMapData(SelectedMap.instance.musicName, SelectedMap.instance.difficultyName);
        LongNoteDisassembly();
        SortNoteData();

        // 1秒間で進む距離
        secondDistance = musicPlayer.ClapSpan() * UserPreference.instance.NoteSpeed();

        // 判定の長さを代入(秒)
        missLen = secondDistance / 60.0f * (float)missFrame;
        goodLen = secondDistance / 60.0f * (float)goodFrame;
        perfectLen = secondDistance / 60.0f * (float)perfectFrame;
    }

    void Update()
    {
        string key = "Lane" + laneNum.ToString();

        // 単発ノーツの判定(ロングノーツの始点を含む)
        if (Input.GetButtonDown(key))
        {
            PlayHitSound();
            NoteJudgment();
            isHoldValid = (isNowLongNote ? true : false);
            keyEffect.EffictStart();

            if (judgmentingLongNote != null)
            {
                noteSetter.AddHoldingLongNote(judgmentingLongNote); // 光らせるロングノーツとして登録
            }
        }

        // ロングノーツの判定
        if (Input.GetButton(key))
        {
            LongNoteJudgment();
            keyEffect.EffictStart();
        }

        // チートタイムのロングノーツ判定
        if (holdUpedCheatTime > 0)
        {
            LongNoteJudgment();
            keyEffect.EffictStart();
        }

        // 叩き損なったノーツの処理
        CheckLostNote();

        if (Input.GetButtonUp(key))
        {
            isHoldValid = true;
            holdUpedCheatTime = goodFrame / 60.0f;

            if (judgmentingLongNote != null)
            {
                noteSetter.RemoveHoldingLongNote(judgmentingLongNote);
            }
        }

        holdUpedCheatTime = (holdUpedCheatTime > 0) ? holdUpedCheatTime - Time.deltaTime : 0;
    }

    // 単発ノーツの判定を行う(ロングノーツの始点を含む)
    private void NoteJudgment()
    {
        foreach (MusicDTO.Note note in mapData.noteList)
        {
            // 判定済み、レーン番号違い、ロングノーツの終点　の場合は判定しない
            if (note.isJudgment || (laneNum != note.lane) || (note.type == 2))
            {
                continue;
            }

            float distance = noteDataConverter.ConvertDistance(note.LPB, note.num);

            if (distance > missLen)
            {
                break;
            }

            // Perfect判定
            if ((distance > -perfectLen) && (distance < perfectLen))
            {
                note.isJudgment = true;
                PerfectJudgmentSetting(distance);
                RemoveNote(note);

                // ロングノーツの始点を判定した場合
                if (note.type == 1)
                {
                    isNowLongNote = true;
                    noteSetter.AddHoldingLongNote(note); // 光らせるロングノーツとして登録
                    judgmentingLongNote = note; // 現在判定中のロングノーツ
                }

                break;
            }

            // Good判定
            if ((distance > -goodLen) && (distance < goodLen))
            {
                note.isJudgment = true;
                GoodJudgmentSetting(distance);
                RemoveNote(note);

                // ロングノーツの始点を判定した場合
                if (note.type == 1)
                {
                    isNowLongNote = true;
                    noteSetter.AddHoldingLongNote(note); // 光らせるロングノーツとして登録
                    judgmentingLongNote = note; // 現在判定中のロングノーツ
                }

                break;
            }

            // Miss判定
            if ((distance > -missLen) && (distance < missLen))
            {
                note.isJudgment = true;
                MissJudgmentSetting(distance);
                RemoveNote(note);

                // ロングノーツの始点を判定した場合
                if (note.type == 1)
                {
                    isNowLongNote = true;
                    isHoldValid = false;
                    judgmentingLongNote = note; // 現在判定中のロングノーツ
                }

                break;
            }
        }
    }

    // ロングノーツの判定を行う
    private void LongNoteJudgment()
    {
        if (isHoldValid == false)
        {
            return;
        }

        foreach (MusicDTO.Note note in mapData.noteList)
        {
            if (note.isJudgment || laneNum != note.lane || note.isLongNote == false || note.type == 1)
            {
                continue;
            }

            float distance = noteDataConverter.ConvertDistance(note.LPB, note.num);

            // Perfect判定
            if (distance <= (secondDistance / 60.0f))
            {
                note.isJudgment = true;
                PerfectJudgmentSetting(distance);

                // ロングノーツの終点を判定した場合
                if (note.type == 2)
                {
                    PlayHitSound();
                    isNowLongNote = false;
                    isHoldValid = false;
                    judgmentingLongNote = null; // 判定中のロングノーツなし
                }
                break;
            }
        }
    }

    // 叩き損なったノーツを判定する
    private void CheckLostNote()
    {
        foreach (MusicDTO.Note note in mapData.noteList)
        {
            if (note.isJudgment || laneNum != note.lane)
            {
                continue;
            }

            float distance = noteDataConverter.ConvertDistance(note.LPB, note.num);

            // Miss判定
            if (distance < -goodLen)
            {
                note.isJudgment = true;
                MissJudgmentSetting(distance);

                // ロングノーツの始点を判定した場合
                if (note.type == 1)
                {
                    isNowLongNote = true;
                    judgmentingLongNote = note; // 現在判定中のロングノーツ
                }
                // ロングノーツの終点を判定した場合
                if (note.type == 2)
                {
                    isNowLongNote = false;
                    judgmentingLongNote = null; // 判定中のロングノーツなし
                }
            }
        }
    }

    /// <summary>
    /// Perfect判定の場合に設定する項目群
    /// </summary>
    /// <param name="distance">判定ラインからの距離</param>
    private void PerfectJudgmentSetting(in float distance)
    {
        judgmentText.PerfectJudgment();
        comboCounter.AddCombo();
        accCounter.AddPerfect();
        scoreCounter.AddPerfect();
        SetTimingText(distance);
    }

    /// <summary>
    /// Good判定の場合に設定する項目群
    /// </summary>
    /// <param name="distance">判定ラインからの距離</param>
    private void GoodJudgmentSetting(in float distance)
    {
        judgmentText.GoodJudgment();
        comboCounter.AddCombo();
        accCounter.AddGood();
        scoreCounter.AddGood();
        SetTimingText(distance);
    }

    /// <summary>
    /// Miss判定の場合に設定する項目群
    /// </summary>
    /// <param name="distance">判定ラインからの距離</param>
    private void MissJudgmentSetting(in float distance)
    {
        judgmentText.MissJudgment();
        comboCounter.ComboZero();
        accCounter.AddMiss();
        timingText.text = "";
        fastSlowText.text = "";
    }

    /// <summary>
    /// 判定済みノーツを削除する
    /// </summary>
    /// <param name="note">削除対象のノーツ</param>
    private void RemoveNote(MusicDTO.Note note)
    {
        // 単発ノーツの場合のみ削除する
        if (note.type == 0)
        {
            playingNoteData.RemoveNote(note);
        }
    }

    /// <summary>
    /// just、fast、slowのテキストを設定する
    /// </summary>
    /// <param name="posY">判定ラインからの距離</param>
    private void SetTimingText(float posY)
    {
        int frame = (int)(posY / (secondDistance / 60.0f));

        if (frame == 0)
        {
            timingText.color = Color.yellow;
            fastSlowText.color = Color.yellow;
            fastSlowText.text = "just";
        }
        else if (frame > 0)
        {
            timingText.color = Color.green;
            fastSlowText.color = Color.green;
            fastSlowText.text = "fast";
        }
        else
        {
            timingText.color = Color.red;
            fastSlowText.color = Color.red;
            fastSlowText.text = "slow";
        }

        timingText.text = frame.ToString();
    }

    // ロングノーツを単発ノーツに分解する
    private void LongNoteDisassembly()
    {
        List<MusicDTO.Note> addNoteList = new List<MusicDTO.Note>();
        foreach (MusicDTO.Note note in mapData.noteList)
        {
            if (note.type == 0)
            {
                continue;
            }

            MusicDTO.Note endNote = new MusicDTO.Note();
            foreach (MusicDTO.Note eNote in note.endNote)
            {
                endNote = eNote;
            }

            int createNoteCnt = endNote.num - note.num;
            // ロングノーツの始点+1のノーツからホールドノーツとして新しく単発ノーツを追加していく
            // ロングノーツの終点まで含めて新しいデータとして追加する仕様(始点ノーツのnote.endNoteは判定に使用しなくなる)
            for (int i = 1; i < createNoteCnt + 1; i++)
            {
                MusicDTO.Note newNote = new MusicDTO.Note();
                newNote.endNote = new List<MusicDTO.Note>();
                newNote.LPB = note.LPB;
                newNote.lane = note.lane;
                newNote.type = (i == createNoteCnt) ? 2 : 3;
                newNote.isJudgment = false;
                newNote.isLongNote = true;
                newNote.num = note.num + i;
                addNoteList.Add(newNote);
            }
        }

        foreach (MusicDTO.Note note in addNoteList)
        {
            mapData.noteList.Add(note);
        }
    }

    // ヒットサウンドを鳴らす
    private void PlayHitSound()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(audioSource.clip);
    }

    // オブジェクトの検索を行う
    private void FindObjects()
    {
        audioSource = this.GetComponent<AudioSource>();
        jsonManager = GameObject.FindGameObjectWithTag("JsonManager").GetComponent<JsonManager>();
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        noteSetter = GameObject.FindGameObjectWithTag("NoteSetter").GetComponent<NoteSetter>();
        playingNoteData = GameObject.FindGameObjectWithTag("PlayingNoteData").GetComponent<PlayingNoteData>();
        noteDataConverter = GameObject.FindGameObjectWithTag("NoteDataConverter").GetComponent<NoteDataConverter>();
    }

    //ノーツデータを昇順ソートする
    private void SortNoteData()
    {
        mapData.noteList.Sort((a, b) => (int)(a.num / (float)a.LPB * 1000) - (int)(b.num / (float)b.LPB * 1000));
    }

    //最大コンボ数(ノーツの総数)を返す
    public int GetMaxComboNum()
    {
        return mapData.noteList.Count;
    }

    // Nullチェックを行う
    private void NullCheck()
    {
        jsonManager.IsNull(nameof(jsonManager));
        musicPlayer.IsNull(nameof(musicPlayer));
        judgmentText.IsNull(nameof(judgmentText));
        audioSource.IsNull(nameof(audioSource));
        comboCounter.IsNull(nameof(comboCounter));
        accCounter.IsNull(nameof(accCounter));
        scoreCounter.IsNull(nameof(scoreCounter));
        keyEffect.IsNull(nameof(keyEffect));
        playingNoteData.IsNull(nameof(playingNoteData));
        noteSetter.IsNull(nameof(noteSetter));
        timingText.IsNull(nameof(timingText));
        noteDataConverter.IsNull(nameof(noteDataConverter));
        fastSlowText.IsNull(nameof(fastSlowText));
    }
}