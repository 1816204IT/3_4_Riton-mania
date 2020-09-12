using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeaderInfo : MonoBehaviour
{
    [SerializeField]
    private Text playerNameText = null;

    void Start()
    {
        if (playerNameText == null)
        {
            Debug.Log("nullを検知");
        }

        SetPlayerNameColor();
    }

    private void Update()
    {
        string playerName = FindObjectOfType<UserAuth>()._playerName;
        if (playerName == null)
        {
            playerName = "NOT LOGIN";
        }
        playerNameText.text = playerName;
    }

    public void SetPlayerNameColor()
    {
        int charaNum = UserPreference._instance._characterNum;
        Color color = CharacterInfoList._instance.GetColor(charaNum);
        playerNameText.color = color;
    }
}
