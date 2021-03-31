using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 曲のシークバー管理クラス
/// </summary>
public class SeekBar : MonoBehaviour
{
    private MusicPlayer musicPlayer = null;
    private RhythmKeeper rhythmKeeper = null;
    private Slider seekBar = null;
    private Color pressedColor = new Color(0.5f, 0.5f, 0.5f);

    void Start()
    {
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        rhythmKeeper = GameObject.Find("RhythmKeeper").GetComponent<RhythmKeeper>();
        seekBar = this.GetComponent<Slider>();
        NullCheck();
    }

    void Update()
    {
        seekBar.value = musicPlayer.GetSeekBarPosition();
    }

    public void AdjustMusicOfSeekBar()
    {
        musicPlayer.AdjustAudioSourceTime(seekBar.value);
    }

    private void NullCheck()
    {
        musicPlayer.IsNull(nameof(musicPlayer));
        rhythmKeeper.IsNull(nameof(rhythmKeeper));
        musicPlayer.IsNull(nameof(musicPlayer));
    }
}
