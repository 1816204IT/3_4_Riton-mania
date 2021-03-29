using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 譜面編集画面管理クラス
/// </summary>
public class EditSceneManager : MonoBehaviour
{
    [SerializeField]
    private MusicPlayButton musicPlayButton = null;
    private NoteDataConverter noteDataConverter = null;

    void Start()
    {
        noteDataConverter = GameObject.FindGameObjectWithTag("NoteDataConverter").GetComponent<NoteDataConverter>();
        
        if (noteDataConverter == null || musicPlayButton == null)
        {
            Debug.Log("nullを検知");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("EditSongSelect");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            musicPlayButton.MusicPlayAndStop();
        }
    }

    public void NoteSpeedUp()
    {
        UserPreference.instance.NoteSpeedUp();
        noteDataConverter.Init();
    }

    public void NoteSpeedDown()
    {
        UserPreference.instance.NoteSpeedDown();
        noteDataConverter.Init();
    }
}