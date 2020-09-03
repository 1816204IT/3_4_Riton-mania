using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//シングルトンクラス
//曲名、BG、音声ファイルを保管
public class MusicInfoList : MonoBehaviour
{
    [SerializeField]
    private string[] musicNames = null;
    [SerializeField]
    private string[] musicEnglishNames = null;  // サーバーのクラスデータが英語でないといけないので曲名の英語版を用意する
    [SerializeField]
    private Sprite[] bgImages = null;
    [SerializeField]
    private AudioClip[] music = null;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    ///====================以下プロパティ====================
    
    public int _musicNum
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

    //シングルトン実態を返す
    public static MusicInfoList _instance
    {
        get;
        private set;
    }
}
