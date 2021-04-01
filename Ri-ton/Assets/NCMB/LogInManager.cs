using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ritonmania;

public class LogInManager : MonoBehaviour
{
    [SerializeField]
    private GameObject logIningObj = null;  // ログイン中に表示するオブジェクト
    [SerializeField]
    private GameObject logOutingObj = null; // ログアウト中に表示するオブジェクト

    [SerializeField]
    private GameObject logInMenu;   // ログインテキスト
    [SerializeField]
    private GameObject createAccountMenu;  // 新規登録テキスト

    [SerializeField]
    private InputField inputFieldID;
    [SerializeField]
    private InputField inputFieldPassword;

    [SerializeField]
    private Text logIningText = null;   // ボタンを押したときの結果を表示するテキストボックス(ログイン成功・失敗等)

    [SerializeField]
    private Text reactionText = null;

    [SerializeField]
    private TitleSceneManager titleSceneManager = null;
    private UserAuth userAuth = null;

    [SerializeField]
    private MoveTween moveTween = null;

    [SerializeField]
    private HeaderInfo headerInfo = null;

    private const float waitTime = 1.0f;  // ログイン完了後に自動でログインボードを消すまでの待ち時間
    private float time = 0.0f;

    // テキストボックスで入力される文字列を格納
    private string id = "";
    private string pw = "";

    LogInState logInState = LogInState.non;
    SignUpState signUpState = SignUpState.non;

    void Start()
    {
        if(logIningObj == null || logOutingObj == null || logInMenu == null || createAccountMenu == null || inputFieldID == null
            || inputFieldPassword == null || logIningText == null || reactionText == null || titleSceneManager == null
            || headerInfo == null)
        {
            Debug.Log("nullを検知");
        }

        userAuth = FindObjectOfType<UserAuth>();
        reactionText.text = "";
    }

    private void Update()
    {
        if (time > 0.0f)
        {
            time -= Time.deltaTime;
            if (time <= 0.0f)
            {
                time = 0.0f;
                RemoveLogInBoard();
            }
        }

        if (logInState == LogInState.trying)
        {
            // ログイン成功
            if (userAuth.logInState == LogInState.succeeded)
            {
                logInState = LogInState.non;
                SucceededLogInFunc();
                reactionText.color = Color.green;
                reactionText.text = "ログイン成功!";
                titleSceneManager.buttonState = ButtonState.IconFetching;
                titleSceneManager.ButtonInit();
                UserPreference.instance.SetCharacterIconName(id);
                UserPreference.instance.CharacterIconFetch(); // サーバーからキャラクター画像を取得
            }
            // ログイン失敗
            if (userAuth.logInState == LogInState.failed)
            {
                logInState = LogInState.non;
                reactionText.color = Color.red;
                reactionText.text = "※ユーザー名またはパスワード\nが違います";
            }
        }

        if (signUpState == SignUpState.trying)
        {
            // サインアップ成功
            if (userAuth.signUpState == SignUpState.succeeded)
            {
                signUpState = SignUpState.non;
                SucceededLogInFunc();
                reactionText.color = Color.green;
                reactionText.text = "ユーザー登録成功!";
                titleSceneManager.buttonState = ButtonState.IconSetting;    // キャラクター画像未登録なのでButtonState.IconSettingとする
                titleSceneManager.ButtonInit();
                UserPreference.instance.SetCharacterIconName(id);
                UserPreference.instance.CharacterIconFetch(); // サーバーからキャラクター画像を取得
            }
            // サインアップ失敗
            if (userAuth.signUpState == SignUpState.failed)
            {
                signUpState = SignUpState.non;
                reactionText.color = Color.red;
                reactionText.text = "※既に使用されているユーザー名です";
            }
        }
    }

    public void InitLogIning()
    {
        logIningObj.SetActive(true);
        logOutingObj.SetActive(false);
        logIningText.text = userAuth.playerName + "  " + "でログイン中";
    }

    public void InitLogOuting()
    {
        logIningObj.SetActive(false);
        logOutingObj.SetActive(true);

        logInMenu.SetActive(true);
        createAccountMenu.SetActive(false);
    }

    // IDが変更されたら
    public void OnValueChangedID()
    {
        id = inputFieldID.text;
    }

    // パスワードが変更されたら
    public void OnValueChangedPassword()
    {
        pw = inputFieldPassword.text;
    }

    // ログインボタンが押されたら
    public void OnClickLogInButton()
    {
        userAuth.logIn(id, pw);
        logInState = LogInState.trying;
    }

    // ログアウトボタンが押されたら
    public void OnClickLogOutButton()
    {
        userAuth.logOut();
        logOutingObj.SetActive(true);
        logIningObj.SetActive(false);
        reactionText.text = "";
        titleSceneManager.buttonState = ButtonState.LogInWaiting;
        titleSceneManager.ButtonInit();
        UserPreference.instance.characterIconLogOut();
    }

    // 新規登録ボタンが押されたら
    public void OnClickSignUpButton()
    {
        userAuth.signUp(id, pw);
        signUpState = SignUpState.trying;
    }

    // 戻るボタンが押されたら
    public void OnClickBackButton()
    {
        createAccountMenu.SetActive(false);
        logInMenu.SetActive(true);
        reactionText.text = "";
    }

    // 新規登録画面に移動するボタンが押されたら
    public void OnClickSignUpMenuButton()
    {
        logInMenu.SetActive(false);
        createAccountMenu.SetActive(true);
        reactionText.text = "";
    }

    // ログインに成功したときの共通処理
    private void SucceededLogInFunc()
    {
        logOutingObj.SetActive(false);
        logIningObj.SetActive(true);
        userAuth.password = inputFieldPassword.text;
        UserPreference.instance.Save();
        logIningText.text = id + "  " + "でログイン中";
        time = waitTime;    // ログイン後の待ち時間
    }

    public void AppearLogInBoard()
    {
        moveTween.Move();
    }

    public void RemoveLogInBoard()
    {
        moveTween.MoveRevert();
    }

    // ログイン中かどうか
    public bool IsLogIn()
    {
        if ((logInState == LogInState.succeeded) || (signUpState == SignUpState.succeeded))
        {
            return true;
        }
        return false;
    }
}
