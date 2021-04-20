using UnityEngine;

/// <summary>
/// シーン内のAudioSorce情報を一括管理するクラス
/// 音量調整が行われた際にシーン内のすべてのAudioSorceのVolumeを変更する
/// シーン遷移した際にもVolume変更を行う
/// </summary>
public class SoundVolumeManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] musicAudioSources = null;
    [SerializeField]
    private AudioSource[] seAudioSources = null;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        ChangedMusicVolume();
        ChangedSEVolume();
    }

    /// <summary>
    /// 曲音量が変更された際の処理
    /// </summary>
    public void ChangedMusicVolume()
    {
        float volume = UserPreference.Instance.MusicVolume;
        foreach(AudioSource audio in musicAudioSources)
        {
            audio.volume = volume;
        }
    }

    /// <summary>
    /// SE音量が変更された際の処理
    /// </summary>
    public void ChangedSEVolume()
    {
        float volume = UserPreference.Instance.SeVolume;
        foreach (AudioSource audio in seAudioSources)
        {
            audio.volume = volume;
        }
    }
}
