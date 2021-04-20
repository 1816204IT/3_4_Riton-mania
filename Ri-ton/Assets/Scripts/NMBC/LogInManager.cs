using UnityEngine;
using UnityEngine.UI;
using Ritonmania;

/// <summary>
/// ログイン管理クラス
/// </summary>
public class LogInManager : MonoBehaviour
{
    [SerializeField]
    private GameObject logIningObj = null;  // ログイン中に表示するオブジェクト
    [SerializeField]
    private GameObject logOutingObj = null; // ログアウト中に表示するオブジェクト

    [SerializeField]
    private GameObject logInMenu;           // ログインテキストボックス
    [SerializeField]
    private GameObject createAccountMenu;   // 新規登録テキストボックス

    [SerializeField]
    private InputField inputFieldID;        // ID入力用フィールド
    [SerializeField]
    private InputField inputFieldPassword;  // パスワード入力用フィールド

    [SerializeField]
    private Text logIningText = null;       //「 〇〇でログイン中」表示用テキストボックス
    [SerializeField]
    private Text reactionText = null;       // ボタン押下時の結果を表示するテキストボックス(ログイン成功・失敗等)

    [SerializeField]
    private TitleSceneManager titleSceneManager = null;
    [SerializeField]
    private MoveTween moveTween = null;     // ログインボードが左右に動くTween

    private const float c_wait_time = 1.0f; // ログイン完了後に自動でログインボードを消すまでの待ち時間

    private UserAuth userAuth = null;
    private float time = 0.0f;
    private LogInState logInState = LogInState.non;
    private SignUpState signUpState = SignUpState.non;
    private string id = "";
    private string pw = "";

    void Start()
    {
        NullCheck();
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
                UserPreference.Instance.SetCharacterIconName(id);
                UserPreference.Instance.CharacterIconFetch(); // サーバーからキャラクター画像を取得
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
                UserPreference.Instance.SetCharacterIconName(id);
                UserPreference.Instance.CharacterIconFetch(); // サーバーからキャラクター画像を取得
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

    /// <summary>
    /// ログイン中の初期化処理
    /// </summary>
    public void InitLogIning()
    {
        logIningObj.SetActive(true);
        logOutingObj.SetActive(false);
        logIningText.text = userAuth.playerName + "  " + "でログイン中";
    }

    /// <summary>
    /// ログアウト中の初期化処理
    /// </summary>
    public void InitLogOuting()
    {
        logIningObj.SetActive(false);
        logOutingObj.SetActive(true);

        logInMenu.SetActive(true);
        createAccountMenu.SetActive(false);
    }

    /// <summary>
    /// IDの入力を検知して更新する
    /// </summary>
    public void OnValueChangedID()
    {
        id = inputFieldID.text;
    }

    /// <summary>
    /// パスワードの入力を検知して更新する
    /// </summary>
    public void OnValueChangedPassword()
    {
        pw = inputFieldPassword.text;
    }

    /// <summary>
    /// ログインボタン押下時の処理
    /// </summary>
    public void OnClickLogInButton()
    {
        userAuth.logIn(id, pw);
        logInState = LogInState.trying;
    }

    /// <summary>
    /// ログアウトボタン押下時の処理
    /// </summary>
    public void OnClickLogOutButton()
    {
        userAuth.logOut();
        logOutingObj.SetActive(true);
        logIningObj.SetActive(false);
        reactionText.text = "";
        titleSceneManager.buttonState = ButtonState.LogInWaiting;
        titleSceneManager.ButtonInit();
        UserPreference.Instance.characterIconLogOut();
    }

    /// <summary>
    /// 新規登録ボタン押下時の処理
    /// </summary>
    public void OnClickSignUpButton()
    {
        userAuth.signUp(id, pw);
        signUpState = SignUpState.trying;
    }

    /// <summary>
    /// 戻るボタン押下時の処理
    /// </summary>
    public void OnClickBackButton()
    {
        createAccountMenu.SetActive(false);
        logInMenu.SetActive(true);
        reactionText.text = "";
    }

    /// <summary>
    /// 新規登録画面へ遷移するボタン押下時の処理
    /// </summary>
    public void OnClickSignUpMenuButton()
    {
        logInMenu.SetActive(false);
        createAccountMenu.SetActive(true);
        reactionText.text = "";
    }

    /// <summary>
    /// ログイン成功時の処理
    /// </summary>
    private void SucceededLogInFunc()
    {
        logOutingObj.SetActive(false);
        logIningObj.SetActive(true);
        userAuth.password = inputFieldPassword.text;
        UserPreference.Instance.Save();
        logIningText.text = id + "  " + "でログイン中";
        time = c_wait_time;    // ログイン後の待ち時間
    }

    /// <summary>
    /// ログインボードを出現させる
    /// </summary>
    public void AppearLogInBoard()
    {
        moveTween.CreateTween();
    }

    /// <summary>
    /// ログインボードを隠す
    /// </summary>
    public void RemoveLogInBoard()
    {
        moveTween.CreateRevertTween();
    }

    /// <summary>
    /// ログイン中かどうかを取得する
    /// </summary>
    public bool IsLogIn()
    {
        if ((logInState == LogInState.succeeded) || (signUpState == SignUpState.succeeded))
        {
            return true;
        }
        return false;
    }

    private void NullCheck()
    {
        logIningObj.IsNull();
        logOutingObj.IsNull();
        logInMenu.IsNull();
        createAccountMenu.IsNull();
        inputFieldID.IsNull();
        inputFieldPassword.IsNull();
        logIningText.IsNull();
        reactionText.IsNull();
        titleSceneManager.IsNull();
    }
}
