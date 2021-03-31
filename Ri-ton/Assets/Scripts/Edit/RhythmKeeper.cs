using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 曲のBPMに合わせて音を鳴らすクラス
/// </summary>
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
        NullCheck();

        pool += musicPlayer._clapSpan;
    }

    void Update()
    {
        float time = musicPlayer.offsetedTime;
        float span = musicPlayer._clapSpan;

        //シークバー操作で曲が巻き戻された場合
        if (time < prevPool)
        {
            pool = time - (time % span) + span;
            prevPool = pool - span;
        }

        //音を鳴らすタイミングか
        if (time >= pool)
        {
            prevPool = pool;
            pool += span;

            if (mapInfoSettings.isOffsetChangeMode)
            {
                audioSource.Play();
            }
        }

        //シークバー操作で曲が進められていた場合
        if (pool < time)
        {
            float ajustTime = time - (time % span);
            pool = time + span;
        }
    }

    private void NullCheck()
    {
        audioSource.IsNull(nameof(audioSource));
        musicPlayer.IsNull(nameof(musicPlayer));
        mapInfoSettings.IsNull(nameof(mapInfoSettings));
    }
}
