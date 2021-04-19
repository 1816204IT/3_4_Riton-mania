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
    private Color pressedColor = Color.gray;

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

    /// <summary>
    /// 曲の再生位置をシークバーの位置に合わせる
    /// </summary>
    public void AdjustMusicOfSeekBar()
    {
        musicPlayer.AdjustAudioSourceTime(seekBar.value);
    }

    private void NullCheck()
    {
        musicPlayer.IsNull();
        rhythmKeeper.IsNull();
        musicPlayer.IsNull();
    }
}
