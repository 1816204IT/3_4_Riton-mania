using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/// <summary>
/// 音楽再生プレイヤークラス
/// </summary>
public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource = null;
    private float BPM = 100.0f;
    private float offset = 0.0f;

    private float audioSourceOldTime = 0.0f;    // Update毎に更新されない
    private float time = 0.0f;  // Update毎に更新される精度の高いAudioSource.time
    private float timeOld;

    private const float startWaitTime = -1.0f;   // 321カウント終了後、0小説目が判定ラインにくるまでの猶予時間
    private bool isPlaying = false;

    private JsonManager jsonManager = null;

    void Awake()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayPause();
        }


        audioSource = this.GetComponent<AudioSource>();
        jsonManager = GameObject.FindGameObjectWithTag("JsonManager").GetComponent<JsonManager>();

        if (audioSource == null || jsonManager == null)
        {
            Debug.Log("nullを検知");
        }

        MapInfo mapInfo;
        if (SceneManager.GetActiveScene().name == "Option")
        {
            mapInfo = jsonManager.LoadMapInfo("castle");
        }
        else
        {
            // DEBUG
            mapInfo = jsonManager.LoadMapInfo("くるくる");
            //mapInfo = jsonManager.LoadMapInfo(SelectedMap._instance._musicName);
            audioSource.clip = MusicInfoList._instance.GetMusic("くるくる");
            //audioSource.clip = MusicInfoList._instance.GetMusic(mapInfo.musicName);
        }

        offset = mapInfo.offset / 100.0f;

        if (SceneManager.GetActiveScene().name != "Edit")
        {
            offset += UserPreference._instance._userOffset;
        }

        BPM = mapInfo.bpm / 100.0f;

        audioSourceOldTime = audioSource.time;
        time = startWaitTime;
        timeOld = time;
    }

    private void Update()
    {
        if (isPlaying)
        {
            time += Time.deltaTime;

            if ( (time >= 0.0f) &&  (timeOld < 0.0f))
            {
                audioSource.Play();
            }

            timeOld = time;
        }

        // audioSorce.timeが更新された時にaudioSourceOldTimeを更新
        // timeをaudioSorce.timeに合わせる
        if (audioSourceOldTime < audioSource.time)
        {
            if (time - Time.deltaTime > audioSource.time)
            {
                // ここでtimeが巻き戻る
            }

            audioSourceOldTime = audioSource.time;
            time = audioSource.time;
        }

        if (SceneManager.GetActiveScene().name != "Edit")
        {
            return;
        }

        float scrollValue = Input.GetAxis("Mouse ScrollWheel");

        if (scrollValue < 0.0f)
        {
            float tmpTime = audioSource.time - 0.2f;
            audioSource.time = (tmpTime < 0) ? 0.0f : tmpTime;
        }
        if (scrollValue > 0.0f)
        {
            float tmpTime = audioSource.time + 0.2f;
            audioSource.time = (tmpTime > audioSource.clip.length) ? audioSource.clip.length : tmpTime;
        }
    }

    //曲の位置からシークバーの位置を計算し値を返す
    public float GetSeekBarPosition()
    {
        return audioSource.time / audioSource.clip.length;
    }

    //シークバーが操作された時に曲の位置を調整する
    public void AdjustAudioSourceTime(float adjustTime)
    {
        audioSource.time = (float)adjustTime * audioSource.clip.length;

        time = audioSource.time;
        timeOld = audioSource.time;
    }

    public void PlayStart()
    {
        //audioSource.Play();
        audioSource.time = 0.0f;
        audioSourceOldTime = 0.0f;
        time = startWaitTime;
        timeOld = time;
        isPlaying = true;
    }

    public void PlayPause()
    {
        isPlaying = false;
        audioSource.Pause();
    }

    public void PlayUnPause()
    {
        isPlaying = true;
        audioSource.Play();
    }

    //曲の再生速度を1.0倍にする
    public void ChangeMusicSpeedDefault()
    {
        audioSource.pitch = 1.0f;
    }

    //曲の再生速度を0.75倍にする
    public void ChangeMusicSpeedThreeQuarter()
    {
        audioSource.pitch = 0.75f;
    }

    //曲の再生速度を0.5倍にする
    public void ChangeMusicSpeedHalf()
    {
        audioSource.pitch = 0.5f;
    }

    //曲の再生速度を0.25倍にする
    public void ChangeMusicSpeedOneQuarter()
    {
        audioSource.pitch = 0.25f;
    }

    ///====================以下プロパティ====================

    // オフセットを考慮した時間(毎フレーム更新しているので更新頻度は高いが低いが不正確で時間が巻き戻るフレームがある)
    // ゲーム中で使用している(時間が巻き戻るフレームがあるが気にならないので現状維持)
    public float offsetedTime
    {
        
        get { return time - offset; }
    }

    // オフセットを考慮した時間(AudioSorce.timeを参照しているので更新頻度が低いが正確)
    // オートに叩かせるときに使用している

    public float _offsetedTimeOrigin
    {
        get { return audioSource.time - offset; }
    }

    public AudioSource _audioSource
    { 
        get { return audioSource; }
    }

    public float _BPM
    {
        get { return BPM; }
        set { BPM = value; }
    }

    public float _clapSpan
    {
        get { return 60.0f / BPM; }
    }

    public float _offset
    {
        get { return offset; }
        set { offset = value; }
    }
}