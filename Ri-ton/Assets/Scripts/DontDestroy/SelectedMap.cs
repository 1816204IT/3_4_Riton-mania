using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DifficultyType
{
    EASY,
    NORMAL,
    HARD,
    EXPERT,
    MAX
}

/// <summary>
/// 現在選択中の譜面(曲名+難易度)を記憶しておくクラス
/// </summary>
public class SelectedMap : MonoBehaviour
{
    private int musicIndex = 0;
    private string musicName = "";
    private string difficultyName = "";

    private DifficultyType nowDifficulty = DifficultyType.EASY;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);

        //起動時にデフォルトで選択される曲と難易度
        musicIndex = 0;

        // DEBUG
        musicName = "くるくる";
        //musicName = MusicInfoList._instance.GetMusicName(musicIndex);
        // DEBUG
        difficultyName = "Easy";
    }

    public int _musicIndex
    {
        get { return musicIndex; }
        set { musicIndex = value; }
    }

    public string _musicName
    {
        get { return musicName; }
        set { musicName = value; }
    }

    public string _difficultyName
    {
        get { return difficultyName; }
        set { difficultyName = value; }
    }

    public DifficultyType _nowDifficulty
    {
        get { return nowDifficulty; }
        set { nowDifficulty = value; }
    }

    public string GetMusicEnglishName()
    {
        return MusicInfoList._instance.GetMusicEnglishName(musicIndex);
    }

    //シングルトン実態を返す
    public static SelectedMap _instance
    {
        get;
        private set;
    }
}
