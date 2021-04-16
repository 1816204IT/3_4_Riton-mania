using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オート君クラス。ノーツのタイミングで音を鳴らします。
/// 譜面編集画面で使用しています。
/// </summary>
public class AutoMachine : MonoBehaviour
{
    [SerializeField]
    private int laneNumer = 0;

    private AudioSource audioSource = null;
    private NoteSetter noteSetter = null;
    private TimingBar timingBar = null;
    private NoteEdit noteEditor = null;
    private MusicPlayer musicPlayer = null;
    private NoteDataConverter noteDataConverter = null;

    private float pool4 = 0.0f;     // 次に音を鳴らすタイミングをプールしていく　1/4間隔
    private float pool6 = 0.0f;     // 次に音を鳴らすタイミングをプールしていく　1/6間隔
    private float pool8 = 0.0f;     // 次に音を鳴らすタイミングをプールしていく　1/8間隔

    private float prevPool4 = 0.0f; // 前に音を鳴らしたタイミングをプールしていく　1/4間隔
    private float prevPool6 = 0.0f; // 前に音を鳴らしたタイミングをプールしていく　1/6間隔
    private float prevPool8 = 0.0f; // 前に音を鳴らしたタイミングをプールしていく　1/8間隔

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        noteSetter = GameObject.FindGameObjectWithTag("NoteSetter").GetComponent<NoteSetter>();
        timingBar = GameObject.FindGameObjectWithTag("TimingBarManager").GetComponent<TimingBar>();
        noteEditor = GameObject.FindGameObjectWithTag("NoteEditor").GetComponent<NoteEdit>();
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        noteDataConverter = GameObject.FindGameObjectWithTag("NoteDataConverter").GetComponent<NoteDataConverter>();

        NullCheck();
    }

    void Update()
    {
        if (musicPlayer.OffsetedTimeOrigin() < 0)
        {
            return;
        }

        ClapCheck(ref pool4, ref prevPool4, 4);
        ClapCheck(ref pool6, ref prevPool6, 6);
        ClapCheck(ref pool8, ref prevPool8, 8);
    }

    /// <summary>
    /// 音を鳴らすタイミングをチェックし、SEを再生する
    /// </summary>
    /// <param name="pool">タイミングプール</param>
    /// <param name="prevPool">前回音を鳴らした時間</param>
    /// <param name="LPB">判定するノーツのLPB</param>
    private void ClapCheck(ref float pool, ref float prevPool, int LPB)
    {
        float time = musicPlayer.OffsetedTimeOrigin();
        float span = (musicPlayer.ClapSpan() / LPB);

        //シークバー操作で曲が巻き戻された場合
        if (time < prevPool)
        {
            float ajustTime = time - (time % span);
            pool = ajustTime + span;
            prevPool = pool - span;
        }

        //音を鳴らすタイミングか
        if (time >= pool)
        {
            int num = noteDataConverter.ConvertBeatNum(time, LPB);
            if (noteSetter.IsNote(LPB, num, laneNumer))
            {
                audioSource.Play();
            }
            //プール値加算
            prevPool = pool;
            pool += span;

            //シークバー操作で曲が進められていた場合
            if (pool < time)
            {
                float ajustTime = time - (time % span);
                pool = ajustTime + span;
            }
        }
    }

    private void NullCheck()
    {
        audioSource.IsNull();
        timingBar.IsNull();
        noteEditor.IsNull();
        musicPlayer.IsNull();
        noteDataConverter.IsNull();
    }
}
