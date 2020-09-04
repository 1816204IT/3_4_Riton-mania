using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;

public enum LogInState
{
    non,
    trying,
    succeeded,
    failed
}
public enum SignUpState
{
    non,
    trying,
    succeeded,
    failed
}

// ユーザー認証
public class UserAuth : MonoBehaviour
{
    private string playerName = null;
    private string password = null;
    private LogInState loginState = LogInState.non;
    private SignUpState signUpState = SignUpState.non;
    
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

    // mobile backendに接続してログイン ------------------------
    public void logIn(string id, string pw)
    {
        loginState = LogInState.trying;

        NCMBUser.LogInAsync(id, pw, (NCMBException e) => 
        {
            // 接続成功したら
            if (e == null)
            {
                loginState = LogInState.succeeded;
                playerName = id;
            }
            // 接続失敗したら
            else
            {
                loginState = LogInState.failed;
            }
        });
    }

    // mobile backendに接続して新規会員登録 ------------------------
    public void signUp(string id, string pw)
    {
        signUpState = SignUpState.trying;

        NCMBUser user = new NCMBUser();
        user.UserName = id;
        user.Password = pw;
        user.SignUpAsync((NCMBException e) => 
        {
            // 新規会員登録に成功したら
            if (e == null)
            {
                signUpState = SignUpState.succeeded;
                playerName = id;
            }
            // 新規会員登録に失敗したら
            else
            {
                signUpState = SignUpState.failed;
            }
        });
    }

    // mobile backendに接続してログアウト ------------------------
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

    public string _playerName
    {
        get { return playerName; }
        set { playerName = value; }
    }

    public string _password
    {
        get { return password; }
        set { password = value; }
    }

    public LogInState _logInState
    { 
        get { return loginState; }
    }

    public SignUpState _signUpState
    {
        get { return signUpState; }
        set { signUpState = value; }
    }
}
