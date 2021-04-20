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
    public static SelectedMap Instance { get; private set; }
    public int MusicIndex { get; set; } = 0;
    public string MusicName { get; set; } = "";
    public string DifficultyName { get; set; } = "";
    public DifficultyType NowDifficulty { get; set; } = DifficultyType.EASY;

    [SerializeField]
    private MusicsInfo musicsInfo = null;

    void Awake()
    {
        musicsInfo.IsNull();

        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        //起動時にデフォルトで選択される曲と難易度
        MusicIndex = 0;

        MusicName = musicsInfo.Info[MusicIndex].musicName;
        DifficultyName = "Easy";
    }

    public string GetMusicEnglishName()
    {
        return musicsInfo.Info[MusicIndex].musicEnglishName;
    }
}