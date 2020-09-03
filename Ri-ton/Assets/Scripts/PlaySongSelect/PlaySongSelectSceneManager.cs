using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlaySongSelectSceneManager : MonoBehaviour
{
    [SerializeField]
    private Text playerName = null;
    [SerializeField]
    private GameObject settingCanvas = null;

    // 自動ログイン時にアイコン画像のフェッチに失敗する場合があるので
    // ログイン後一定秒数毎にアイコン画像が設定されているかチェックする
    private const float iconImageChackTime = 1.0f;
    private float timer = 0.0f;

    private void Start()
    {
        TitleSceneManager._prevSceneName = "PlaySongSelect";
        
        if (playerName == null || settingCanvas == null)
        {
            Debug.Log("nullを検知");
        }

        playerName.text = FindObjectOfType<UserAuth>()._playerName;

        // カーソルの表示をONにする
        Cursor.visible = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingCanvas.activeSelf)
            {
                settingCanvas.SetActive(false);
                return;
            }
            SceneManager.LoadScene("Title");
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnClickPlayButton();
        }

        // キャラクター画像が正しく設定されているか
        if (UserPreference._instance._characterNum != 5)
        {
            return;
        }

        timer += Time.deltaTime;
        if (timer >= iconImageChackTime)
        {
            timer = 0.0f;
            UserPreference._instance.CharacterIconFetch();
        }
    }

    public void OnClickPlayButton()
    {
        SceneManager.LoadScene("Play");
    }

    public void OnClickBackButton()
    {
        if (settingCanvas.activeSelf)
        {
            settingCanvas.SetActive(false);
            // ユーザー設定をローカルデータとして保存
            UserPreference._instance.Save();
            return;
        }
        SceneManager.LoadScene("Title");
    }

    public void OnClickSettingButton()
    {
        if (settingCanvas.activeSelf == false)
        {
            settingCanvas.SetActive(true);
        }
    }
}
