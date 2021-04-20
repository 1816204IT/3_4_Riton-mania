using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 曲開始前のカウントダウンを表示する
/// </summary>
public class PlayStartTimer : MonoBehaviour
{
    public bool IsTutorialEnd { get; set; } = false;

    private const float c_count_down_time = 3.0f;   // 3・2・1のカウントダウン時間

    private Text text = null;
    private MusicPlayer musicPlayer = null;
    private float timer = c_count_down_time;

    void Start()
    {
        text = this.GetComponent<Text>();
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        NullCheck();

        int timeText = ((int)timer * 10 + 19) / 10;
        text.text = timeText.ToString();
    }

    void Update()
    {
        if ( (timer <= 0.0f) || (IsTutorialEnd == false) )
        {
            return;
        }

        timer -= Time.deltaTime;
        int timeText = ((int)timer * 10 + 19) / 10;
        text.text = timeText.ToString();

        if (timer <= 0.0f)
        {
            text.enabled = false;

            if (musicPlayer.AudioSource.time <= 0.0f)
            {
                musicPlayer.PlayStart();
            }
            else
            {
                musicPlayer.PlayUnPause();
            }
        }
    }

    /// <summary>
    /// カウントダウンタイマーを初期化する
    /// </summary>
    public void TimerReset()
    {
        timer = c_count_down_time;
        text.enabled = true;
    }

    private void NullCheck()
    {
        text.IsNull();
        musicPlayer.IsNull();
    }
}
