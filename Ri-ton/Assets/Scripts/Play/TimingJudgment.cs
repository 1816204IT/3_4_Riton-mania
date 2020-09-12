using NoteEditor.DTO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

//プレイヤがノーツを叩いた際に判定を行う
public class TimingJudgment : MonoBehaviour
{
    [SerializeField]
    private int laneNum = 0;
    [SerializeField]
    private JudgmentText judgmentText = null;
    [SerializeField]
    private KeyEffect keyEffect = null;
    [SerializeField]
    private ComboCounter comboCounter = null;
    [SerializeField]
    private AccCounter accCounter = null;
    [SerializeField]
    private ScoreCounter scoreCounter = null;
    [SerializeField]
    private Text timingText = null;
    [SerializeField]
    private Text fastOrSlowText = null;

    private JsonManager jsonManager = null;
    private MusicPlayer musicPlayer = null;
    private PlayingNoteData playingNoteData = null;
    private NotesSetter notesSetter = null;
    private AudioSource audioSource = null;
    private NoteDataConverter noteDataConverter = null;

    private MusicDTO.MapData mapData = new MusicDTO.MapData();
    float secondDistance = 0.0f; //1秒でどれだけ譜面が進むか？
    private const int perfectFrame = 6; 
    private const int goodFrame = 10; 
    private const int missFrame = 14; 
    private float perfectLen = 0.0f;
    private float goodLen = 0.0f;
    private float missLen = 0.0f;

    // ロングノーツ関連
    private bool isNowLongNote = false;         // ロングノーツの始点から終点の間のみtureとする
    private bool isHoldValid = false;           // 長押しの際にロングノーツとの判定を取るか(連続したロングノーツや、ロングノーツの始点前からホールドしていた際のチェック用)
    private float holdUpedCheatTime = 0.0f;     // ホールドを離した時の曲の時間
    private MusicDTO.Note judgmentingLongNote;  // 判定中のロングノーツ

    [SerializeField]
    private PlaySceneManager playSceneManager = null;

    void Start()
    {
        jsonManager = GameObject.FindGameObjectWithTag("JsonManager").GetComponent<JsonManager>();
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        playingNoteData = GameObject.FindGameObjectWithTag("PlayingNoteData").GetComponent<PlayingNoteData>();
        notesSetter = GameObject.FindGameObjectWithTag("NotesSetter").GetComponent<NotesSetter>();
        audioSource = this.GetComponent<AudioSource>();
        noteDataConverter = GameObject.FindGameObjectWithTag("NoteDataConverter").GetComponent<NoteDataConverter>();

        if (jsonManager == null || musicPlayer == null || judgmentText == null || audioSource == null
            || comboCounter == null || accCounter == null || scoreCounter == null || keyEffect == null
            || playingNoteData == null || notesSetter == null || timingText == null || noteDataConverter == null
            || playSceneManager == null || fastOrSlowText == null)
        {
            Debug.Log("nullを検知");
        }

        mapData = jsonManager.LoadMapData(SelectedMap._instance._musicName, SelectedMap._instance._difficultyName);
        LongNoteDisassembly();
        SortNoteData();

        //1秒でどれだけ譜面が進むか？
        secondDistance = musicPlayer._clapSpan * UserPreference._instance._notesSpeed;
        //判定の長さを代入
        perfectLen = secondDistance / 60.0f * (float)perfectFrame;
        goodLen = secondDistance / 60.0f * (float)goodFrame;
        missLen = secondDistance / 60.0f * (float)missFrame;

        //Debug.Log("ノーツ速度 [ " + UserPreference._instance._notesSpeed + " ]");
        //Debg.Log("PerfectDistance = " + perfectLen);
        //Debug.Log("goodDistance = " + goodLen);
        //Debug.Log("missDistance = " + missLen);

        Debug.Log("secondDistance = " + secondDistance.ToString());
        Debug.Log("frameDistance = " + (secondDistance / 60.0f).ToString());
    }

    void Update()
    {
        if (playSceneManager._isTutorialEnd == false)
        {
            return;
        }
       
        string key = "Lane" + laneNum.ToString();
        // 単発ノーツの判定(ロングノーツの始点を含む)
        if (Input.GetButtonDown(key))
        {
            audioSource.Stop();
            audioSource.PlayOneShot(audioSource.clip);
            NoteJudgment();
            isHoldValid = isNowLongNote ? true : false;
            keyEffect.EffictStart();

            if (judgmentingLongNote != null)
            {
                notesSetter.AddHoldingLongNote(judgmentingLongNote); // 光らせるロングノーツとして登録
            }
        }
        // ロングノーツの判定(押しっぱなしノーツ)
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
        //叩き損なったノーツの処理
        CheckLostNote();

        if (Input.GetButtonUp(key))
        {
            isHoldValid = true;
            holdUpedCheatTime = goodFrame / 60.0f;

            if (judgmentingLongNote != null)
            {
                notesSetter.RemoveHoldingLongNote(judgmentingLongNote);
            }
        }
        holdUpedCheatTime =  (holdUpedCheatTime > 0) ? holdUpedCheatTime - Time.deltaTime : 0;
    }

    private void NoteJudgment()
    {
        foreach (MusicDTO.Note note in mapData.notes)
        {
            if (note.isJudgment || laneNum != note.lane || note.type == 2)
            {
                continue;
            }

            float distance = noteDataConverter.ConvertDistance(note.LPB, note.num);

            if (distance > missLen)
            {  
                break;
            }

            // Perfect判定
            if ((distance > - perfectLen) && (distance < perfectLen))
            {
                note.isJudgment = true;
                judgmentText.PerfectJudgment();
                comboCounter.AddCombo();
                accCounter.AddPerfect();
                scoreCounter.AddPerfect();
                SetTimingText(distance);  // タイミングのズレ
                RemoveNote(note);   // 叩いたノーツを消す

                // ロングノーツの始点を判定した場合
                if (note.type == 1)
                {
                    isNowLongNote = true;
                    notesSetter.AddHoldingLongNote(note); // 光らせるロングノーツとして登録
                    judgmentingLongNote = note; // 現在判定中のロングノーツ
                }

                break;
            }
            // Good判定
            if ((distance > -goodLen) && (distance < goodLen))
            {
                note.isJudgment = true;
                judgmentText.GoodJudgment();
                comboCounter.AddCombo();
                accCounter.AddGood();
                scoreCounter.AddGood();
                SetTimingText(distance);  // タイミングのズレ
                RemoveNote(note);   // 叩いたノーツを消す

                // ロングノーツの始点を判定した場合
                if (note.type == 1)
                {
                    isNowLongNote = true;
                    notesSetter.AddHoldingLongNote(note); // 光らせるロングノーツとして登録
                    judgmentingLongNote = note; // 現在判定中のロングノーツ
                }

                break;
            }
            // Miss判定
            if ((distance > -missLen) && (distance < missLen))
            {
                note.isJudgment = true;
                judgmentText.MissJudgment();
                comboCounter.ComboZero();
                accCounter.AddMiss();
                SetTimingText(distance);  // タイミングのズレ
                RemoveNote(note);   // 叩いたノーツを消す

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

    private void LongNoteJudgment()
    {
        if (isHoldValid == false)
        {
            return;
        }

        foreach (MusicDTO.Note note in mapData.notes)
        {
            if (note.isJudgment || laneNum != note.lane || note.isLongNote == false || note.type == 1)
            {
                continue;
            }

            float distance = noteDataConverter.ConvertDistance(note.LPB, note.num);

            // Perfect判定
            if (distance <= (secondDistance / 60.0f))
            {
                Debug.Log(distance);
                Debug.Log(secondDistance / 60.0f);

                note.isJudgment = true;
                judgmentText.PerfectJudgment();
                comboCounter.AddCombo();
                accCounter.AddPerfect();
                scoreCounter.AddPerfect();
                SetTimingText(distance);  // タイミングのズレ

                // ロングノーツの終点を判定した場合
                if (note.type == 2)
                {
                    isNowLongNote = false;
                    isHoldValid = false;
                    // SE再生
                    audioSource.Stop();
                    audioSource.PlayOneShot(audioSource.clip);
                    judgmentingLongNote = null; // 判定中のロングノーツなし
                }
                break;
            }
        }
    }

    private void CheckLostNote()
    {
        foreach (MusicDTO.Note note in mapData.notes)
        {
            if (note.isJudgment || laneNum != note.lane)
            {
                continue;
            }

            float distance = noteDataConverter.ConvertDistance(note.LPB, note.num);

            // Miss判定
            if (distance <  - goodLen)
            {
                note.isJudgment = true;
                judgmentText.MissJudgment();
                comboCounter.ComboZero();
                accCounter.AddMiss();
                timingText.text = "";

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

    private void RemoveNote(MusicDTO.Note note)
    {
        // 単発ノーツに限り叩いたノーツを消す
        if (note.type == 0)
        {
            playingNoteData.RemoveNote(note);
        }
    }

    private void SetTimingText(float posY)
    {
        int frame = (int)(posY / (secondDistance / 60.0f));
        if (frame == 0)
        {
            timingText.color = Color.yellow;
            fastOrSlowText.color = Color.yellow;
            fastOrSlowText.text = "just";
        }
        else if (frame > 0)
        {
            timingText.color = Color.green;
            fastOrSlowText.color = Color.green;
            fastOrSlowText.text = "fast";
        }
        else
        {
            timingText.color = Color.red;
            fastOrSlowText.color = Color.red;
            fastOrSlowText.text = "slow";
        }
        timingText.text = frame.ToString();
    }

    //ロングノーツを単発ノーツに分解する
    private void LongNoteDisassembly()
    {
        List<MusicDTO.Note> addNoteList = new List<MusicDTO.Note>();
        foreach (MusicDTO.Note note in mapData.notes)
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
            mapData.notes.Add(note);
        }
    }

    //ノーツデータを昇順ソートする
    private void SortNoteData()
    {
        mapData.notes.Sort((a, b) => (int)(a.num / (float)a.LPB * 1000) - (int)(b.num / (float)b.LPB * 1000));
    }

    //最大コンボ数(ノーツの総数)を返す
    public int GetMaxComboNum()
    {
        return mapData.notes.Count;
    }

    public float _perfectLen
    {
        get { return perfectLen; }
    }

    public float _goodLen
    { 
        get { return goodLen; }
    }
}
