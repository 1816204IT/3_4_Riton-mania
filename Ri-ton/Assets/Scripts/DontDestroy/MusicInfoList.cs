using UnityEngine;

/// <summary>
/// 曲名、BG、音声ファイルを保管
/// </summary>
public class MusicInfoList : MonoBehaviour
{
    public static MusicInfoList Instance { get; private set; }

    [SerializeField]
    private string[] musicNames = null;
    [SerializeField]
    private string[] musicEnglishNames = null;  // サーバーのクラスデータ名が英語でないといけないので英語版の曲名を用意する
    [SerializeField]
    private Sprite[] bgImages = null;
    [SerializeField]
    private AudioClip[] musics = null;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    
    public int MusicNum()
    {
        return musicNames.Length;
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
        return musics[musicIndex];
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
        return musics[i];
    }
}
