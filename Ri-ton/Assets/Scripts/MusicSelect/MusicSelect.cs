using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 曲選択クラス
/// </summary>
public class MusicSelect : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioPlayer = null;

    private int musicNameIndex = 0;

    void Start()
    {
        NullCheck();
        SetNewMusic(SelectedMap.Instance.MusicIndex);
    }

    /// <summary>
    /// 曲名を変更する
    /// </summary>
    /// <param name="inMusicNameIndex">曲番号</param>
    public void SetNewMusic(int inMusicNameIndex)
    {
        musicNameIndex = inMusicNameIndex;
        audioPlayer.clip = MusicInfoList.Instance.GetMusic(inMusicNameIndex);
        audioPlayer.Play();
    }

    /// <summary>
    /// 曲編集シーンへ遷移する
    /// </summary>
    public void SceneChangeToEdit()
    {
        SceneManager.LoadScene("Edit");
    }

    /// <summary>
    /// プレイシーンへ遷移する
    /// </summary>
    public void SceneChangeToPlay()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "PlaySongSelect")
        {
            SceneManager.LoadScene("Play");
        }
        else if (sceneName == "EditSongSelect")
        {
            SceneManager.LoadScene("Edit");
        }
        else
        {
            Debug.Log("無効なシーン名です");
        }
    }

    private void NullCheck()
    {
        audioPlayer.IsNull();
    }
}