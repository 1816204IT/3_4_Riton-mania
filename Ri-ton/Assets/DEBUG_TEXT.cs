using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DEBUG_TEXT : MonoBehaviour
{
    public Text text = null;
    UserAuth userAuth;

    void Start()
    {
        if (text == null)
        {
            Debug.Log("nullを検知");
        }

        userAuth = FindObjectOfType<UserAuth>();
    }

    void Update()
    {
        string charaName = userAuth._playerName;
        int charaNum = UserPreference._instance._characterNum;
        text.text = "キャラクター番号 = " + charaNum.ToString() + "\n" +
                    "プレイヤ名 = " + charaName.ToString();
    }
}
