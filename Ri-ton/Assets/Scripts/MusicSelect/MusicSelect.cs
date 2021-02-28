using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 曲選択クラス
/// </summary>
public class MusicSelect : MonoBehaviour
{
    private int musicNameIndex = 0;
    private string diffName = "";

    [SerializeField]
    private AudioSource audioPlayer = null;

    void Start()
    {
        if (audioPlayer == null)
        {
            Debug.Log("nullを検知");
        }

        diffName = SelectedMap._instance._difficultyName;
        SetNewMusic(SelectedMap._instance._musicIndex);
    }

    public void SetNewMusic(int inMusicNameIndex)
    {
        musicNameIndex = inMusicNameIndex;
        audioPlayer.clip = MusicInfoList._instance.GetMusic(inMusicNameIndex);
        audioPlayer.Play();
    }

    //====================ボタン押下時のイベント====================
    public void SceneChangeToEdit()
    {
        SceneManager.LoadScene("Edit");
    }

    public void SceneChangeToPlay()
    {
        SelectedMap._instance._musicName = MusicInfoList._instance.GetMusicName(musicNameIndex);
        SelectedMap._instance._difficultyName = diffName;

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

    public string _diffName
    {
        get { return diffName; }
    }
}
