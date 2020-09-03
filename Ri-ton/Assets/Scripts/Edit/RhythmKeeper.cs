using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmKeeper : MonoBehaviour
{
    private MusicPlayer musicPlayer = null;
    private AudioSource audioSource = null;
    private MapInfoSettings mapInfoSettings = null;
    private float pool = 0.0f; //次に音を鳴らすタイミングをプールしていく
    private float prevPool = 0.0f;

    void Start()
    {
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        audioSource = this.GetComponent<AudioSource>();
        mapInfoSettings = GameObject.FindGameObjectWithTag("MapInfoSettings").GetComponent<MapInfoSettings>();

        if (audioSource == null || musicPlayer == null || mapInfoSettings == null)
        {
            Debug.Log("nullを検知");
        }

        pool += musicPlayer._clapSpan;
    }

    void Update()
    {
        if (musicPlayer.offsetedTime >= pool)
        {
            prevPool = pool;
            pool += musicPlayer._clapSpan;

            if (mapInfoSettings._isOffsetChangeMode)
            {
                audioSource.Play();
            }
        }

        //シークバーが操作された時にPoolの値を調整する
        if (musicPlayer.offsetedTime < prevPool)
        {
            pool = musicPlayer.offsetedTime - (musicPlayer.offsetedTime % musicPlayer._clapSpan);
        }
    }
}
