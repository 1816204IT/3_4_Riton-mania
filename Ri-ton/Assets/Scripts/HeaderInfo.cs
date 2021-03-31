using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ヘッダー情報表示クラス
/// プレイヤ名
/// </summary>
public class HeaderInfo : MonoBehaviour
{
    [SerializeField]
    private Text playerNameText = null;

    void Start()
    {
        NullCheck();
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
        int charaNum = UserPreference.instance._characterNum;
        Color color = CharacterInfoList.instance.GetColor(charaNum);
        playerNameText.color = color;
    }

    private void NullCheck()
    {
	    playerNameText.IsNull(nameof(playerNameText));
    }
}
