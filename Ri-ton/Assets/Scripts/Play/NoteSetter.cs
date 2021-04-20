using NoteEditor.DTO;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 譜面データを元にノーツを配置する
/// </summary>
public class NoteSetter : MonoBehaviour
{
    [SerializeField]
    private NoteColor noteColor = null;
    [SerializeField]
    private Transform judgmentBarTransform = null;

    public struct LongNoteInfo
    {
        public List<GameObject> objList;                // ロングノーツのGameObjectを保持する
        public List<SpriteRenderer> spriteRendererList; // ロングノーツのSpriteRendererをリストとして保持する(ロングノーツを発行させる際に使用する)
    }

    private LongNoteInfo evenNumberNongNoteInfo;
    private LongNoteInfo oddNumberNongNoteInfo;
    private List<MusicDTO.Note> holdingLongNoteList = new List<MusicDTO.Note>();

    // PlayingNoteDataクラスが持っているnowMapDataの参照
    private MusicDTO.MapData mapData = new MusicDTO.MapData();
    // ノーツオブジェクトのリスト
    private List<GameObject> evenNumberNoteList = new List<GameObject>();
    private List<GameObject> oddNumberNoteList = new List<GameObject>();

    private MusicPlayer musicPlayer = null;
    private JsonManager jsonManager = null;
    private NoteDataConverter noteDataConverter = null;
    private PlayingNoteData playingNoteData = null;

    void Start()
    {
        FindObjects();
        NullCheck();
        // 譜面データの参照を読み込む
        mapData = playingNoteData.GetNowMapData();
    }

    void Update()
    {
        // 全てのノーツを画面外に移動(setActiveは重い処理のようなので座標移動で誤魔化す)
        MoveNoteOutOfScreen(evenNumberNoteList);
        MoveNoteOutOfScreen(oddNumberNoteList);
        MoveNoteOutOfScreen(evenNumberNoteList);
        LongNoteMoveOutOfScreen(evenNumberNongNoteInfo.objList);
        LongNoteMoveOutOfScreen(oddNumberNongNoteInfo.objList);

        // 全てのロングノーツの色をリセット
        ResetLongNoteBrightness(ref evenNumberNongNoteInfo.spriteRendererList, noteColor.EvenLongDefault);
        ResetLongNoteBrightness(ref oddNumberNongNoteInfo.spriteRendererList, noteColor.OddLongDefault);

        SetNote();
        SetLongNote();
    }

    /// <summary>
    /// ゲームオブジェクトの検索
    /// </summary>
    private void FindObjects()
    {
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        noteDataConverter = GameObject.FindGameObjectWithTag("NoteDataConverter").GetComponent<NoteDataConverter>();
        jsonManager = GameObject.FindGameObjectWithTag("JsonManager").GetComponent<JsonManager>();
        playingNoteData = GameObject.FindGameObjectWithTag("PlayingNoteData").GetComponent<PlayingNoteData>();

        evenNumberNongNoteInfo.objList = new List<GameObject>();
        evenNumberNongNoteInfo.spriteRendererList = new List<SpriteRenderer>();
        oddNumberNongNoteInfo.objList = new List<GameObject>();
        oddNumberNongNoteInfo.spriteRendererList = new List<SpriteRenderer>();

        FindNoteObjects(ref evenNumberNoteList, "NoteEvenNumber");
        FindNoteObjects(ref oddNumberNoteList, "NoteOddNumber");
        FindNoteObjects(ref evenNumberNongNoteInfo.objList, "LongNoteEvenNumber");
        FindNoteObjects(ref oddNumberNongNoteInfo.objList, "LongNoteOddNumber");

        AddSpriteRendererList(ref evenNumberNongNoteInfo);
        AddSpriteRendererList(ref oddNumberNongNoteInfo);
    }

    /// <summary>
    /// ヒエラルキーに存在するノーツオブジェクトを検索しリストに追加する
    /// </summary>
    /// <param name="list">検索対象を追加するリスト</param>
    /// <param name="tagName">検索対象のタグ名</param>
    private void FindNoteObjects(ref List<GameObject> list, string tagName)
    {
        var findObjects = GameObject.FindGameObjectsWithTag(tagName);
        if (findObjects.Length == 0)
        {
            Debug.Log("nullを検知");
        }
        foreach (GameObject note in findObjects)
        {
            list.Add(note);
        }
    }

    /// <summary>
    /// ロングノーツのSpriteRendererをリストとして保持する
    /// </summary>
    private void AddSpriteRendererList(ref LongNoteInfo info)
    {
        foreach (GameObject obj in info.objList)
        {
            info.spriteRendererList.Add(obj.GetComponent<SpriteRenderer>());
        }
    }

    /// <summary>
    /// ノーツを画面外へ移動させる
    /// </summary>
    /// <param name="list">移動対象のノーツのリスト</param>
    private void MoveNoteOutOfScreen(List<GameObject> list)
    {
        foreach (GameObject note in list)
        {
            var tmpPos = note.transform.position;
            note.transform.position = new Vector3(tmpPos.x, 3500, tmpPos.z);
        }
    }

    /// <summary>
    /// ロングノーツを画面外へ移動させ、サイズの縮小を行う
    /// </summary>
    /// <param name="list">移動対象のノーツリスト</param>
    private void LongNoteMoveOutOfScreen(List<GameObject> list)
    {
        foreach (GameObject note in list)
        {
            var tmpPos = note.transform.position;
            note.transform.position = new Vector3(tmpPos.x, 3500, tmpPos.z);
            var tmpScale = note.transform.localScale;
            note.transform.localScale = new Vector3(tmpScale.x, 50, tmpScale.z);
        }
    }

    /// <summary>
    /// ロングノーツの色を初期化する
    /// </summary>
    /// <param name="color">初期化色</param>

    private void ResetLongNoteBrightness(ref List<SpriteRenderer> srList, Color color)
    {
        foreach (SpriteRenderer sr in srList)
        {
            sr.color = color;
        }
    }

    /// <summary>
    /// 単発ノーツを設置していく
    /// </summary>
    private void SetNote()
    {
        int usedEvenNumberNoteNum = 0;
        int usedOddNumberNoteNum = 0;

        foreach (MusicDTO.Note note in mapData.noteList)
        {
            // 単発ノーツの配置
            if ((note.lane == 1) || (note.lane == 2))
            {
                SetNoteFunc(note, ref evenNumberNoteList, ref usedEvenNumberNoteNum);
            }
            else
            {
                SetNoteFunc(note, ref oddNumberNoteList, ref usedOddNumberNoteNum);
            }

            // ロングノーツの終点の配置(単発ノーツとして配置)
            foreach (MusicDTO.Note endNote in note.endNote)
            {
                if ((note.lane == 1) || (note.lane == 2))
                {
                    SetNoteFunc(endNote, ref evenNumberNoteList, ref usedEvenNumberNoteNum);
                }
                else
                {
                    SetNoteFunc(endNote, ref oddNumberNoteList, ref usedOddNumberNoteNum);
                }
            }
        }
    }
    
    /// <summary>
    /// ノーツ配置の共通処理
    /// </summary>
    /// <param name="note">配置するノーツ情報</param>
    /// <param name="list">ノーツオブジェクトリスト</param>
    /// <param name="usedNoteNum">既に使用しているノーツオブジェクト数</param>
    private void SetNoteFunc(MusicDTO.Note note, ref List<GameObject> list, ref int usedNoteNum)
    {
        int num = 0;
        foreach (GameObject n in list)
        {
            num++;
            if (num > usedNoteNum)
            {
                Vector3 tmpPos = n.transform.position;
                float posX = UserPreference.Instance.NotePosXOfLaneZero() + note.lane * UserPreference.Instance.NoteSizeX();
                float posY = noteDataConverter.ConvertDistance(note.LPB, note.num);
                posY += judgmentBarTransform.position.y;
                if (posY > 0 && posY < 2400)
                {
                    n.transform.position = new Vector3(posX, posY, tmpPos.z);
                    usedNoteNum++;
                }
                break;
            }
        }
    }

    /// <summary>
    /// ロングノーツを設置していく
    /// </summary>
    private void SetLongNote()
    {
        //画面に表示するロングノーツをvalidNoteListに入れていく
        List<MusicDTO.Note> validNoteList = new List<MusicDTO.Note>();
        foreach (MusicDTO.Note note in mapData.noteList)
        {
            if (note.type == 0)
            {
                continue;
            }

            float noteStartPosY = noteDataConverter.ConvertDistance(note.LPB, note.num) + judgmentBarTransform.position.y;
            float noteEndPosY = 0;
            foreach (MusicDTO.Note endNote in note.endNote)
            {
                noteEndPosY = noteDataConverter.ConvertDistance(endNote.LPB, endNote.num) + judgmentBarTransform.position.y;
            }

            //ロングノーツが画面内にない場合は処理しない
            if ((noteStartPosY > 2400) || (noteEndPosY < 0))
            {
                continue;
            }

            validNoteList.Add(note);
        }

        int usedEvenNumberNoteNum = 0;
        int usedOddNumberNoteNum = 0;

        foreach (MusicDTO.Note note in validNoteList)
        {
            if ((note.lane == 1) || (note.lane == 2))
            {
                SetLongNoteFunc(note, ref evenNumberNongNoteInfo, ref usedEvenNumberNoteNum, true);
            }
            else
            {
                SetLongNoteFunc(note, ref oddNumberNongNoteInfo, ref usedOddNumberNoteNum, false);
            }
        }
    }

    /// <summary>
    /// ロングノーツ配置の共通処理
    /// </summary>
    /// <param name="note">配置するノーツ情報</param>
    /// <param name="list">ノーツオブジェクトリスト</param>
    /// <param name="usedNoteNum">既に使用しているロングノーツオブジェクト数</param>
    /// <param name="isEvenNumber">偶数レーンかどうか</param>
    private void SetLongNoteFunc(MusicDTO.Note note, ref LongNoteInfo list, ref int usedNoteNum, bool isEvenNumber)
    {
        int num = 0;

        for (int i = 0; i < list.objList.Count; i++)
        {
            num++;
            if (num > usedNoteNum)
            {
                float scale = 0;
                list.objList[i].transform.position = GetLongNotePosition(note, ref scale);
                var tmpScale = list.objList[i].transform.localScale;
                list.objList[i].transform.localScale = new Vector3(tmpScale.x, scale, tmpScale.z);
                usedNoteNum++;

                if (IsLongNoteHolding(note))
                {
                    Color color = (isEvenNumber) ? noteColor.EvenLongHolding : noteColor.OddLongHolding;
                    list.spriteRendererList[i].color = color;
                }

                break;
            }
        }
    }

    /// <summary>
    /// ロングノーツがホールド中かを取得する
    /// </summary>
    private bool IsLongNoteHolding(MusicDTO.Note note)
    {
        foreach (MusicDTO.Note n in holdingLongNoteList)
        {
            // 同一ノーツがあるかチェック
            if ((note.lane == n.lane) && (note.LPB == n.LPB) && (note.num == n.num))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// ロングノーツの座標(中心座標)を返す
    /// </summary>
    /// <param name="note">対象ロングノーツ</param>
    /// <param name="scale">ロングノーツのscale値</param>
    private Vector3 GetLongNotePosition(MusicDTO.Note note, ref float scale)
    {
        float startNotePosY = noteDataConverter.ConvertDistance(note.LPB, note.num);
        startNotePosY += judgmentBarTransform.position.y;

        MusicDTO.Note eNote = new MusicDTO.Note();
        foreach (MusicDTO.Note endNote in note.endNote)
        {
            eNote = endNote;
        }

        float endNotePosY = noteDataConverter.ConvertDistance(eNote.LPB, eNote.num);
        endNotePosY += judgmentBarTransform.position.y;
        scale = endNotePosY - startNotePosY + 1;
        float notePosX = UserPreference.Instance.NotePosXOfLaneZero() + note.lane * UserPreference.Instance.NoteSizeX();
        return new Vector3(notePosX, (startNotePosY + scale / 2), 1); ;
    }

    /// <summary>
    /// 指定した時間指定したレーンにノーツがあるかを判定する
    /// オートプレイで使用しています
    /// </summary>
    public bool IsNote(int LPB, int num, int lane)
    {
        foreach (MusicDTO.Note note in mapData.noteList)
        {
            float a = num / (float)LPB;
            float b = note.num / (float)note.LPB;
            if (a == b)
            {
                if (note.lane == lane)
                {
                    return true;
                }
            }

            foreach (MusicDTO.Note endNote in note.endNote)
            {
                float c = num / (float)LPB;
                float d = endNote.num / (float)endNote.LPB;
                if (c == d)
                {
                    if (endNote.lane == lane)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    /// <summary>
    /// ホールド中ロングノーツをリストに追加する
    /// </summary>
    /// <param name="note">追加対象ロングノーツ</param>
    public void AddHoldingLongNote(MusicDTO.Note note)
    {
        foreach (MusicDTO.Note n in holdingLongNoteList)
        {
            // 同一位置のノーツをはじく
            if ((note.lane == n.lane) && (note.LPB == n.LPB) && (note.num == n.num))
            {
                return;
            }
        }
        // ノーツの追加
        holdingLongNoteList.Add(note);
    }

    /// <summary>
    /// ホールド中ロングノーツリストから対象ノーツを除外する
    /// </summary>
    /// <param name="note">除外対象ロングノーツ</param>
    public void RemoveHoldingLongNote(MusicDTO.Note note)
    {
        // 同一ノーツをはじく
        for (int i = 0; i < holdingLongNoteList.Count; i++)
        {
            var n = holdingLongNoteList[i];
            // 同一位置のノーツをはじく
            if ((note.lane == n.lane) && (note.LPB == n.LPB) && (note.num == n.num))
            {
                holdingLongNoteList.RemoveAt(i);
            }
        }
    }

    private void NullCheck()
    {
        musicPlayer.IsNull();
        jsonManager.IsNull();
        noteDataConverter.IsNull();
        playingNoteData.IsNull();
        judgmentBarTransform.IsNull();
        noteColor.IsNull();
    }
}