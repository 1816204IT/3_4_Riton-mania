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

    [SerializeField]
    private AudioSource audioPlayer = null;

    void Start()
    {
        if (audioPlayer == null)
        {
            Debug.Log("nullを検知");
        }

        //diffName = SelectedMap.instance._difficultyName;
        SetNewMusic(SelectedMap.instance._musicIndex);
    }

    public void SetNewMusic(int inMusicNameIndex)
    {
        musicNameIndex = inMusicNameIndex;
        audioPlayer.clip = MusicInfoList.instance.GetMusic(inMusicNameIndex);
        audioPlayer.Play();
    }

    //====================ボタン押下時のイベント====================
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
}