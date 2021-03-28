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

        SelectedMap.instance._musicName = "castle";
        SelectedMap.instance._difficultyName = "Easy";
        offsetText.text = UserPreference.instance._userOffset.ToString();
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
        UserPreference.instance.NotesSpeedUp();
    }

    public void NotesSpeedDown()
    {
        UserPreference.instance.NotesSpeedDown();
    }

    public void AddOffset()
    {
        UserPreference.instance.AddOffset();
        offsetText.text = UserPreference.instance._userOffset.ToString();
    }

    public void SubtractOffset()
    {
        UserPreference.instance.SubtractOffset();
        offsetText.text = UserPreference.instance._userOffset.ToString();
    }
}
