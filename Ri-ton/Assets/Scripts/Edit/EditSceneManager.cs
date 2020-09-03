using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
            SceneManager.LoadScene(TitleSceneManager._prevSceneName);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            musicPlayButton.MusicPlayAndStop();
        }
    }

    public void NotesSpeedUp()
    {
        UserPreference._instance.NotesSpeedUp();
        noteDataConverter.Init();
    }

    public void NotesSpeedDown()
    {
        UserPreference._instance.NotesSpeedDown();
        noteDataConverter.Init();
    }
}
