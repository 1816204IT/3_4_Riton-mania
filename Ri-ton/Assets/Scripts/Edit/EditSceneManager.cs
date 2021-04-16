using UnityEngine;
using UnityEngine.SceneManagement;

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
        NullCheck();
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

    /// <summary>
    /// ノーツ速度を上げる
    /// </summary>
    public void NoteSpeedUp()
    {
        UserPreference.Instance.NoteSpeedUp();
        noteDataConverter.Init();
    }

    /// <summary>
    /// ノーツ速度を下げる
    /// </summary>
    public void NoteSpeedDown()
    {
        UserPreference.Instance.NoteSpeedDown();
        noteDataConverter.Init();
    }

    private void NullCheck()
    {
        noteDataConverter.IsNull();
        musicPlayButton.IsNull();
    }
}