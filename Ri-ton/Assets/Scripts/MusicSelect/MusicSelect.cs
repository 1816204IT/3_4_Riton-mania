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
        SetNewMusic(SelectedMap.instance.musicIndex);
    }

    public void SetNewMusic(int inMusicNameIndex)
    {
        musicNameIndex = inMusicNameIndex;
        audioPlayer.clip = MusicInfoList.instance.GetMusic(inMusicNameIndex);
        audioPlayer.Play();
    }

    public void SceneChangeToEdit()
    {
        SceneManager.LoadScene("Edit");
    }

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
        audioPlayer.IsNull(nameof(audioPlayer));
    }
}