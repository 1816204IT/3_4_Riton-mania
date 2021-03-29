using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NoteEditor.DTO;

/// <summary>
/// プレイ中のノーツデータを管理する
/// プレイ中に判定されたノーツを排除していく
/// </summary>
public class PlayingNoteData : MonoBehaviour
{
    private JsonManager jsonManager = null;
    private MusicDTO.MapData nowMapData = new MusicDTO.MapData();

    void Awake()
    {
        jsonManager = GameObject.FindGameObjectWithTag("JsonManager").GetComponent<JsonManager>();

        if (jsonManager == null)
        {
            Debug.Log("nullを検知");
        }

        // ノーツデータの読み込み
        nowMapData = jsonManager.LoadMapData(SelectedMap.instance._musicName, SelectedMap.instance._difficultyName);
    }

    public ref MusicDTO.MapData GetNowMapData()
    {
        return ref nowMapData;
    }

    //ノーツデータを追加する
    public void AddNote(MusicDTO.Note note)
    {
        if (IsNoteExist(note))
        {
            return;
        }
        //ノーツを追加&昇順ソートしてセーブ
        nowMapData.noteList.Add(note);
        nowMapData.noteList.Sort((a, b) => (int)(a.num / (float)a.LPB * 1000) - (int)(b.num / (float)b.LPB * 1000));
        //セーブ
        SaveNoteData();
    }

    //追加するノーツデータに被りがないかチェックする
    private bool IsNoteExist(MusicDTO.Note note)
    {
        foreach (MusicDTO.Note n in nowMapData.noteList)
        {
            //同一位置のノーツをはじく
            float a = n.num / (float)n.LPB;
            float b = note.num / (float)note.LPB;
            if ((a == b) && (n.lane == note.lane))
            {
                return true;
            }
        }
        return false;
    }

    //ノーツデータを削除する
    public void RemoveNote(MusicDTO.Note note)
    {
        foreach (MusicDTO.Note n in nowMapData.noteList)
        {
            //同一位置のノーツを削除
            float a = n.num / (float)n.LPB;
            float b = note.num / (float)note.LPB;
            if ((a == b) && (n.lane == note.lane))
            {
                nowMapData.noteList.Remove(n);
                break;
            }
        }
    }

    public void SaveNoteData()
    {
        jsonManager.SaveNoteData(nowMapData, SelectedMap.instance._musicName, SelectedMap.instance._difficultyName);
    }
}
