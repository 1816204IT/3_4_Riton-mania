using System.Collections;
using System.Collections.Generic;
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
        ChangeMusicVolume();
        ChangeSEVolume();
    }

    public void ChangeMusicVolume()
    {
        float volume = UserPreference._instance._musicVolume;
        foreach(AudioSource audio in musicAudioSources)
        {
            audio.volume = volume;
        }
    }

    public void ChangeSEVolume()
    {
        float volume = UserPreference._instance._seVolume;
        foreach (AudioSource audio in seAudioSources)
        {
            audio.volume = volume;
        }
    }
}
