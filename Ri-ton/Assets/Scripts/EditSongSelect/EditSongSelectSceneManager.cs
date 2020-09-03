using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EditSongSelectSceneManager : MonoBehaviour
{
    private void Start()
    {
        TitleSceneManager._prevSceneName = "EditSongSelect";
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
