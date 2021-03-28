using System.Collections;
using System.Collections.Generic;
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

        if (button == null || image == null || musicPlayer == null)
        {
            Debug.Log("nullを検知");
        }
    }

    void Update()
    {
        if (musicPlayer.audioSource.isPlaying)
        {
            image.sprite = stopButton;
        }
        else
        {
            image.sprite = playButton;
        }
    }

    public void MusicPlayAndStop()
    {
        if (musicPlayer.audioSource.isPlaying)
        {
            musicPlayer.audioSource.Pause();
        }
        else
        {
            if (musicPlayer.audioSource.time == 0.0f)
            {
                musicPlayer.audioSource.Play();
            }
            else
            {
                musicPlayer.audioSource.UnPause();
            }
        }
    }
}
