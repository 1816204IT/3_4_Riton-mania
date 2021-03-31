using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 曲名、BG、音声ファイルを保管
/// </summary>
public class MusicInfoList : MonoBehaviour
{
    public static MusicInfoList instance { get; private set; }

    [SerializeField]
    private string[] musicNames = null;
    [SerializeField]
    private string[] musicEnglishNames = null;  // サーバーのクラスデータ名が英語でないといけないので英語版の曲名を用意する
    [SerializeField]
    private Sprite[] bgImages = null;
    [SerializeField]
    private AudioClip[] music = null;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    
    public int MusicNum
    {
        get { return musicNames.Length; }
    }

    public string GetMusicName(int musicIndex)
    {
        return musicNames[musicIndex];
    }

    public string GetMusicEnglishName(int musicIndex)
    {
        return musicEnglishNames[musicIndex];
    }

    public Sprite GetBgImage(int musicIndex)
    {
        return bgImages[musicIndex];
    }

    public Sprite GetBgImage(string musicName)
    {
        int i = 0;
        foreach (string name in musicNames)
        {
            if (name == musicName)
            {
                break;
            }
            i++;
        }

        if (i == musicNames.Length)
        {
            Debug.Log("無効な曲名です");
        }
        return bgImages[i];
    }

    public AudioClip GetMusic(int musicIndex)
    {
        return music[musicIndex];
    }

    public AudioClip GetMusic(string musicName)
    {
        int i = 0;
        foreach (string name in musicNames)
        {
            if (name == musicName)
            {
                break;
            }
            i++;
        }

        if (i == musicNames.Length)
        {
            Debug.Log("無効な曲名です");
        }
        return music[i];
    }
}
