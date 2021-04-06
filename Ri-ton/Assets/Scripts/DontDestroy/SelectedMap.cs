using UnityEngine;
using Ritonmania;

namespace Ritonmania
{
    public enum DifficultyType
    {
        EASY,
        NORMAL,
        HARD,
        EXPERT,
        MAX
    }
}

/// <summary>
/// 現在選択中の譜面(曲名+難易度)を記憶しておくクラス
/// </summary>
public class SelectedMap : MonoBehaviour
{
    public static SelectedMap instance { get; private set; }
    public int musicIndex { get; set; } = 0;
    public string musicName { get; set; } = "";
    public string difficultyName { get; set; } = "";
    public DifficultyType nowDifficulty { get; set; } = DifficultyType.EASY;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        //起動時にデフォルトで選択される曲と難易度
        musicIndex = 0;

        musicName = MusicInfoList.instance.GetMusicName(musicIndex);
        difficultyName = "Easy";
    }

    public string GetMusicEnglishName()
    {
        return MusicInfoList.instance.GetMusicEnglishName(musicIndex);
    }
}