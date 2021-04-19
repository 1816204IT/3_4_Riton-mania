using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 曲再生ボタンクラス
/// </summary>
public class MusicPlayButton : MonoBehaviour
{
    [SerializeField]
    private Sprite playButton = null;
    [SerializeField]
    private Sprite stopButton = null;

    private Button button = null;
    private Image image = null;
    private MusicPlayer musicPlayer = null;

    void Start()
    {
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        button = this.GetComponent<Button>();
        image = this.GetComponent<Image>();
        NullCheck();
    }

    void Update()
    {
        if (musicPlayer.AudioSource.isPlaying)
        {
            image.sprite = stopButton;
        }
        else
        {
            image.sprite = playButton;
        }
    }

    /// <summary>
    /// 曲の再生と停止
    /// </summary>
    public void MusicPlayAndStop()
    {
        if (musicPlayer.AudioSource.isPlaying)
        {
            musicPlayer.AudioSource.Pause();
        }
        else
        {
            if (musicPlayer.AudioSource.time == 0.0f)
            {
                musicPlayer.AudioSource.Play();
            }
            else
            {
                musicPlayer.AudioSource.UnPause();
            }
        }
    }

    private void NullCheck()
    {
        button.IsNull();
        image.IsNull();
        musicPlayer.IsNull();
    }
}
