using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayStartTimer : MonoBehaviour
{
    private Text text = null;
    private MusicPlayer musicPlayer = null;

    float timer = 3.0f;

    void Start()
    {
        text = this.GetComponent<Text>();
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();

        if (text == null || musicPlayer == null)
        {
            Debug.Log("nullを検知");
        }

        int timeText = ((int)timer * 10 + 19) / 10;
        text.text = timeText.ToString();
    }

    void Update()
    {
        if (timer <= 0.0f)
        {
            return;
        }

        timer -= Time.deltaTime;
        int timeText = ((int)timer * 10 + 19) / 10;
        text.text = timeText.ToString();

        if (timer <= 0.0f)
        {
            text.enabled = false;

            if (musicPlayer._audioSource.time <= 0.0f)
            {
                musicPlayer.PlayStart();
            }
            else
            {
                musicPlayer.PlayUnPause();
            }
        }
    }

    public void TimerReset()
    {
        timer = 3.0f;
        text.enabled = true;
    }
}
