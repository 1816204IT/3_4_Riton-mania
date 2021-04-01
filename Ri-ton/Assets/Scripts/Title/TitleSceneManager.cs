using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Ritonmania;

public enum ButtonState
{
    LogInWaiting,   // ログイン待ち状態(ログインボタンを表示)
    IconSetting,    // アイコンが登録されてない状態(キャラクター選択ボタンを表示)
    IconFetching,   // サーバーにアイコン番号をフェッチ中
    Normal,         // 通常状態(プレイボタン、キャラクター選択ボタン、ログアウトボタンを表示)
}

/// <summary>
/// タイトルシーン管理クラス
/// </summary>
public class TitleSceneManager : MonoBehaviour
{
    public static string prevSceneName { get; set; } = ""; 
    public ButtonState buttonState { get; set; } = ButtonState.LogInWaiting;

    [SerializeField]
    private GameObject logIningObj = null;  // ログイン中に表示するオブジェクト
    [SerializeField]
    private GameObject logOutingObj = null; // ログアウト中に表示するオブジェクト
    [SerializeField]
    private GameObject iconSettingObj = null;

    [SerializeField]
    private SoundVolumeManager soundVolumeManager = null;
    [SerializeField]
    private AudioSource bgm = null;

    private void Start()
    {
        NullCheck();
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
        else if (buttonState == ButtonState.IconFetching)
        {
            logIningObj.SetActive(false);
            logOutingObj.SetActive(false);
            iconSettingObj.SetActive(false);
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
        if (buttonState == ButtonState.Normal)
        {
            return;
        }

        if (buttonState == ButtonState.IconFetching)
        {
            // アイコンフェッチ終了したか
            if (UserPreference.instance.GetIconFetchState() == FetchState.succeeded)
            {
                int characterNum = UserPreference.instance.GetCharacterNumber();

                // キャラクター番号が正常に設定されているか
                if ( (characterNum >= 0) && (characterNum <= 4) )
                {
                    buttonState = ButtonState.Normal;
                    ButtonInit();
                    soundVolumeManager.Init();  // ローカルから読み込んだユーザー設定を元に音量調節を行う
                    if (bgm.isPlaying == false)
                    {
                        PlayBGM();
                    }
                }
                else
                {
                    buttonState = ButtonState.IconSetting;
                    ButtonInit();
                    if (bgm.isPlaying == false)
                    {
                        PlayBGM();
                    }
                }

            }
        }

        // キャラクター画像が正しく設定されているか
        if (UserPreference.instance.GetCharacterNumber() != 5)
        {
            return;
        }

        // アイコンフェッチ中か
        if (buttonState == ButtonState.IconFetching)
        {
            FetchState nowFecthState = UserPreference.instance.GetIconFetchState();
            if ((nowFecthState == FetchState.non) || (nowFecthState == FetchState.failed))
            {
                UserPreference.instance.CharacterIconFetch();
            }
            return;
        }
    }

    public void PlayBGM()
    {
        bgm.Play(); // BGM再生スタート
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

    private void NullCheck()
    {
        logIningObj.IsNull(nameof(logIningObj));
        logIningObj.IsNull(nameof(logIningObj));
        iconSettingObj.IsNull(nameof(iconSettingObj));
        soundVolumeManager.IsNull(nameof(soundVolumeManager));
        bgm.IsNull(nameof(bgm));
    }
}
