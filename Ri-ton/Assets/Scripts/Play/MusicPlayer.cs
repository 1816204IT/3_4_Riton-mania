using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 音楽再生プレイヤークラス
/// </summary>
public class MusicPlayer : MonoBehaviour
{
    public AudioSource AudioSource { get; private set; } = null;
    public float Bpm { get; set; } = 0.0f;
    public float Offset { get; set; } = 0.0f;

    private const float c_start_wait_time = 1.0f;   // 321カウント終了後、0小説目が判定ラインにくるまでの猶予時間

    private float audioSourceOldTime = 0.0f;    // Update毎に更新されない
    private float time = 0.0f;  // Update毎に更新される精度の高いAudioSource.time
    private float timeOld;
    private bool isPlaying = false;
    private JsonManager jsonManager = null;

    void Awake()
    {
        AudioSource = this.GetComponent<AudioSource>();
        jsonManager = GameObject.FindGameObjectWithTag("JsonManager").GetComponent<JsonManager>();
        NullCheck();

        MapInfo mapInfo;
        if (SceneManager.GetActiveScene().name == "Option")
        {
            mapInfo = jsonManager.LoadMapInfo("castle");
        }
        else
        {
            mapInfo = jsonManager.LoadMapInfo(SelectedMap.Instance.MusicName);
            AudioSource.clip = MusicInfoList.Instance.GetMusic(mapInfo.musicName);
        }

        Offset = mapInfo.offset / 100.0f;

        if (SceneManager.GetActiveScene().name != "Edit")
        {
            Offset += UserPreference.Instance.UserOffset();
        }

        Bpm = mapInfo.bpm / 100.0f;

        audioSourceOldTime = AudioSource.time;
        time = -c_start_wait_time;
        timeOld = time;
    }

    private void Update()
    {
        if (isPlaying)
        {
            time += Time.deltaTime;

            if ( (time >= 0.0f) &&  (timeOld < 0.0f))
            {
                AudioSource.Play();
            }

            timeOld = time;
        }

        // audioSorce.timeが更新された時にaudioSourceOldTimeを更新
        // timeをaudioSorce.timeに合わせる
        if (audioSourceOldTime < AudioSource.time)
        {
            if (time - Time.deltaTime > AudioSource.time)
            {
                // ここでtimeが巻き戻る
            }

            audioSourceOldTime = AudioSource.time;
            time = AudioSource.time;
        }

        if (SceneManager.GetActiveScene().name != "Edit")
        {
            return;
        }

        float scrollValue = Input.GetAxis("Mouse ScrollWheel");

        if (scrollValue < 0.0f)
        {
            float tmpTime = AudioSource.time - 0.2f;
            AudioSource.time = (tmpTime < 0) ? 0.0f : tmpTime;
        }
        if (scrollValue > 0.0f)
        {
            float tmpTime = AudioSource.time + 0.2f;
            AudioSource.time = (tmpTime > AudioSource.clip.length) ? AudioSource.clip.length : tmpTime;
        }
    }

    //曲の位置からシークバーの位置を計算し値を返す
    public float GetSeekBarPosition()
    {
        return AudioSource.time / AudioSource.clip.length;
    }

    //シークバーが操作された時に曲の位置を調整する
    public void AdjustAudioSourceTime(float adjustTime)
    {
        AudioSource.time = (float)adjustTime * AudioSource.clip.length;

        time = AudioSource.time;
        timeOld = AudioSource.time;
    }

    public void PlayStart()
    {
        //audioSource.Play();
        AudioSource.time = 0.0f;
        audioSourceOldTime = 0.0f;
        time = -c_start_wait_time;
        timeOld = time;
        isPlaying = true;
    }

    public void PlayPause()
    {
        isPlaying = false;
        AudioSource.Pause();
    }

    public void PlayUnPause()
    {
        isPlaying = true;
        AudioSource.Play();
    }

    //曲の再生速度を1.0倍にする
    public void ChangeMusicSpeedDefault()
    {
        AudioSource.pitch = 1.0f;
    }

    //曲の再生速度を0.75倍にする
    public void ChangeMusicSpeedThreeQuarter()
    {
        AudioSource.pitch = 0.75f;
    }

    //曲の再生速度を0.5倍にする
    public void ChangeMusicSpeedHalf()
    {
        AudioSource.pitch = 0.5f;
    }

    //曲の再生速度を0.25倍にする
    public void ChangeMusicSpeedOneQuarter()
    {
        AudioSource.pitch = 0.25f;
    }

    ///====================以下プロパティ====================

    // オフセットを考慮した時間(毎フレーム更新しているので更新頻度は高いが低いが不正確で時間が巻き戻るフレームがある)
    // ゲーム中で使用している(時間が巻き戻るフレームがあるが気にならないので現状維持)
    public float offsetedTime
    {
        
        get { return time - Offset; }
    }

    // オフセットを考慮した時間(AudioSorce.timeを参照しているので更新頻度が低いが正確)
    // オートに叩かせるときに使用している

    public float OffsetedTimeOrigin()
    {
        return AudioSource.time - Offset;
    }

    public float ClapSpan()
    {
        return 60.0f / Bpm;
    }

    private void NullCheck()
    {
        AudioSource.IsNull();
        jsonManager.IsNull();
    }
}