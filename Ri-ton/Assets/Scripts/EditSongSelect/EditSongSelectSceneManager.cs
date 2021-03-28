using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 編集譜面選択画面の管理クラス
/// </summary>
public class EditSongSelectSceneManager : MonoBehaviour
{
    private void Start()
    {
        TitleSceneManager.prevSceneName = "EditSongSelect";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Title");
        }
    }

    public void OnClickEditButton()
    {
        SceneManager.LoadScene("Edit");
    }
}
