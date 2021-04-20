using UnityEngine;
using NCMB;
using Ritonmania;

namespace Ritonmania
{
    public enum LogInState
    {
        non,        // ログアウト中
        trying,     // ログイン接続中
        succeeded,  // ログイン成功
        failed      // ログイン失敗
    }
    public enum SignUpState
    {
        non,        // サインアップしてない状態
        trying,     // サインアップ中(新規ユーザーデータ作成中)
        succeeded,  // サインアップ成功
        failed      // サインアップ失敗
    }

}

/// <summary>
/// ユーザー認証クラス
/// </summary>
public class UserAuth : MonoBehaviour
{
    public string playerName { get; set; } = null;
    public string password { get; set; } = null;
    public LogInState logInState { get; set; } = LogInState.non;
    public SignUpState signUpState { get; set; } = SignUpState.non;
    
    private UserAuth instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            string name = gameObject.name;
            gameObject.name = name + "(Singleton)";

            GameObject duplicater = GameObject.Find(name);
            if (duplicater != null)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.name = name;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// mobile backendに接続してログインする
    /// </summary>
    /// <param name="id">ユーザーID</param>
    /// <param name="pw">ユーザーパスワード</param>
    public void logIn(string id, string pw)
    {
        logInState = LogInState.trying;

        NCMBUser.LogInAsync(id, pw, (NCMBException e) => 
        {
            // 接続成功したら
            if (e == null)
            {
                logInState = LogInState.succeeded;
                playerName = id;
            }
            // 接続失敗したら
            else
            {
                logInState = LogInState.failed;
            }
        });
    }

    /// <summary>
    /// mobile backendに接続して新規会員登録する
    /// </summary>
    /// <param name="id">ユーザーID</param>
    /// <param name="pw">ユーザーパスワード</param>
    public void signUp(string id, string pw)
    {
        signUpState = SignUpState.trying;

        NCMBUser user = new NCMBUser();
        user.UserName = id;
        user.Password = pw;
        user.SignUpAsync((NCMBCallback)((NCMBException e) => 
        {
            // 新規会員登録に成功したら
            if (e == null)
            {
                this.signUpState = SignUpState.succeeded;
                playerName = id;
            }
            // 新規会員登録に失敗したら
            else
            {
                this.signUpState = SignUpState.failed;
            }
        }));
    }

    /// <summary>
    /// mobile backendに接続してログアウトする
    /// </summary>
    public void logOut()
    {
        NCMBUser.LogOutAsync((NCMBException e) => 
        {
            if (e == null)
            {
                playerName = null;
            }
        });
    }
}
