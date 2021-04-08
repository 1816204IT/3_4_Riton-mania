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
        NullCheck();

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

    private void NullCheck()
    {
        jsonManager.IsNull();
    }
}