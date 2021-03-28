using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイトルシーンにてログインの管理をする
/// 過去にログイン履歴があるか等からどのフローへ進むか判断する
/// </summary>
public class LoginFlow : MonoBehaviour
{
    [SerializeField]
    private LogInManager logInManager = null;
    [SerializeField]
    private TitleSceneManager titleSceneManager = null;

    bool isOnce = false;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        if (logInManager == null || titleSceneManager == null)
        {
            Debug.Log("nullを検知");
        }
    }

    private void Update()
    {
        if (isOnce)
        {
            return;
        }

        // ゲーム起動時かどうか
        bool isGameStart;
        if (FindObjectOfType<UserAuth>()._playerName == null)
        {
            isGameStart = true;
        }
        else
        {
            isGameStart = false;
        }

        // 過去にユーザー登録を行っているか
        bool isUserRegistered = false;

        // ゲーム起動時なら、ローカルデータのユーザー設定を取得する
        if (isGameStart)
        {
            isUserRegistered = UserPreference.instance.Load();
        }

        // ゲーム起動時かつユーザー登録を1度も行っていないなら
        if (isGameStart && (isUserRegistered == false))
        {
            logInManager.InitLogOuting();
            titleSceneManager.buttonState = ButtonState.LogInWaiting;
            titleSceneManager.ButtonInit();
            titleSceneManager.PlayBGM();
        }
        // ゲーム起動時ではない、またはゲーム起動時だが過去にユーザー登録を行っている
        else
        {
            logInManager.InitLogIning();
            titleSceneManager.buttonState = ButtonState.IconFetching;
            titleSceneManager.ButtonInit();
        }

        isOnce = true;
    }
}
