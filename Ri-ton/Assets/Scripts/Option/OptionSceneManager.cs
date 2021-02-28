using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// オプション画面管理クラス
/// </summary>
public class OptionSceneManager : MonoBehaviour
{
    [SerializeField]
    private Text offsetText = null;

    void Awake()
    {
        if (offsetText == null)
        {
            Debug.Log("nullを検知");
        }

        SelectedMap._instance._musicName = "castle";
        SelectedMap._instance._difficultyName = "Easy";
        offsetText.text = UserPreference._instance._userOffset.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Title");
        }
    }

    public void NotesSpeedUp()
    {
        UserPreference._instance.NotesSpeedUp();
    }

    public void NotesSpeedDown()
    {
        UserPreference._instance.NotesSpeedDown();
    }

    public void AddOffset()
    {
        UserPreference._instance.AddOffset();
        offsetText.text = UserPreference._instance._userOffset.ToString();
    }

    public void SubtractOffset()
    {
        UserPreference._instance.SubtractOffset();
        offsetText.text = UserPreference._instance._userOffset.ToString();
    }
}
