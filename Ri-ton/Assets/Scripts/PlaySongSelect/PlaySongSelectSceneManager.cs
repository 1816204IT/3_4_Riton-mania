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

    [SerializeField]
    private Image myCharacter = null;

    private void Start()
    {
        TitleSceneManager._prevSceneName = "PlaySongSelect";
        
        if (playerName == null || settingCanvas == null || myCharacter == null)
        {
            Debug.Log("nullを検知");
        }

        playerName.text = FindObjectOfType<UserAuth>()._playerName;

        // カーソルの表示をONにする
        Cursor.visible = true;

        // キャラクター表示
        int charaNum = UserPreference._instance._characterNum;
        myCharacter.sprite = CharacterInfoList._instance.GetSprite(charaNum);
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

        if ( Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) )
        {
            OnClickPlayButton();
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
