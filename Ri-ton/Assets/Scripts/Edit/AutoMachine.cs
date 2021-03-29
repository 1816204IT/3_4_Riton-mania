using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オート君クラス
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

    private float pool4 = 0.0f; //次に音を鳴らすタイミングをプールしていく　1/4間隔
    private float pool6 = 0.0f; //次に音を鳴らすタイミングをプールしていく　1/6間隔
    private float pool8 = 0.0f; //次に音を鳴らすタイミングをプールしていく　1/8間隔

    private float prevPool4 = 0.0f;
    private float prevPool6 = 0.0f;
    private float prevPool8 = 0.0f;

    private int clappedNum = 0;

    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        noteSetter = GameObject.FindGameObjectWithTag("NoteSetter").GetComponent<NoteSetter>();
        timingBar = GameObject.FindGameObjectWithTag("TimingBarManager").GetComponent<TimingBar>();
        noteEditor = GameObject.FindGameObjectWithTag("NoteEditor").GetComponent<NoteEdit>();
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        noteDataConverter = GameObject.FindGameObjectWithTag("NoteDataConverter").GetComponent<NoteDataConverter>();

        if (audioSource == null || timingBar == null || noteEditor == null
             || musicPlayer == null || noteDataConverter == null)
        {
            Debug.Log("nullを検知");
        }
    }

    void Update()
    {
        if (musicPlayer._offsetedTimeOrigin < 0)
        {
            return;
        }

        bool isClaped = false;
        ClapCheck(ref pool4, ref prevPool4, 4, ref isClaped);
        ClapCheck(ref pool6, ref prevPool6, 6, ref isClaped);
        ClapCheck(ref pool8, ref prevPool8, 8, ref isClaped);
    }

    private void ClapCheck(ref float pool, ref float prevPool, int LPB, ref bool isClaped)
    {
        float time = musicPlayer._offsetedTimeOrigin;
        float span = (musicPlayer._clapSpan / LPB);

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
                if (isClaped == false)
                {
                    audioSource.Play();
                    isClaped = true;
                    clappedNum++;
                }
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
}
