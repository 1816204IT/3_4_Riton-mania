using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum ButtonState
{
    LogInWaiting,   // ログイン待ち状態(ログインボタンを表示)
    IconSetting,    // アイコンが登録されてない状態(キャラクター選択ボタンを表示)
    Normal,         // 通常状態(プレイボタン、キャラクター選択ボタン、ログアウトボタンを表示)
}

public class TitleSceneManager : MonoBehaviour
{
    private static string prevSceneName = "";

    [SerializeField]
    private GameObject logIningObj = null;  // ログイン中に表示するオブジェクト
    [SerializeField]
    private GameObject logOutingObj = null; // ログアウト中に表示するオブジェクト
    [SerializeField]
    private GameObject iconSettingObj = null;

    private ButtonState buttonState = ButtonState.LogInWaiting;

    // 自動ログイン時にアイコン画像のフェッチに失敗する場合があるので
    // ログイン後一定秒数毎にアイコン画像が設定されているかチェックする
    private const float iconImageChackTime = 1.0f; 
    private float timer = 0.0f;

    private void Start()
    {
        if (logIningObj == null || logIningObj == null || iconSettingObj == null)
        {
            Debug.Log("nullを検知");
        }
    }

    public void ButtonInit()
    {
        if (buttonState == ButtonState.LogInWaiting)
        {
            logIningObj.SetActive(false);
            logOutingObj.SetActive(true);
            iconSettingObj.SetActive(false);
        }
        else if (buttonState == ButtonState.IconSetting)
        {
            logIningObj.SetActive(false);
            logOutingObj.SetActive(false);
            iconSettingObj.SetActive(true);
        }
        else if (buttonState == ButtonState.Normal)
        {
            logIningObj.SetActive(true);
            logOutingObj.SetActive(false);
            iconSettingObj.SetActive(false);
        }
    }

    private void Update()
    {
        // ログイン後か
        if (buttonState != ButtonState.Normal)
        {
            return;
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

    public void ChangeEditSongSelectScene()
    {
        SceneManager.LoadScene("EditSongSelect");
    }

    public void ChangePlaySongSelectScene()
    {
        SceneManager.LoadScene("PlaySongSelect");
    }

    public void ChangeCharacterSelectScene()
    {
        SceneManager.LoadScene("CharacterSelect");
    }

    public ButtonState _buttonState
    {
        set { buttonState = value; }
    }

    public static string _prevSceneName
    {
        get { return prevSceneName; }
        set { prevSceneName = value; }
    }
}
