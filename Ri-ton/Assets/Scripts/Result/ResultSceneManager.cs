using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// リザルト画面管理クラス
/// </summary>
public class ResultSceneManager : MonoBehaviour
{
    private JsonManager jsonManager = null;
   void Start()
    {
        jsonManager = GameObject.FindGameObjectWithTag("JsonManager").GetComponent<JsonManager>();
        if (jsonManager == null)
        {
            Debug.Log("nullを検知");
        }

        // カーソルの表示をONにする
        Cursor.visible = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("PlaySongSelect");
        }
    }

    public void OnClickBackButton()
    {
        SceneManager.LoadScene("PlaySongSelect");
    }
}