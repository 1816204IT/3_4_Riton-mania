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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("PlaySongSelect");
        }
    }

    /// <summary>
    /// 戻るボタン押下時に曲選択画面へ遷移する
    /// </summary>
    public void OnClickBackButton()
    {
        SceneManager.LoadScene("PlaySongSelect");
    }

    private void NullCheck()
    {
        jsonManager.IsNull();
    }
}