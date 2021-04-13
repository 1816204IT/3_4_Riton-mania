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
        string playerName = FindObjectOfType<UserAuth>().playerName;
        if (playerName == null)
        {
            playerName = "NOT LOGIN";
        }
        playerNameText.text = playerName;
    }

    public void SetPlayerNameColor()
    {
        int charaNum = UserPreference.Instance.GetCharacterNumber();
        Color color = CharacterInfoList.instance.GetColor(charaNum);
        playerNameText.color = color;
    }

    private void NullCheck()
    {
	    playerNameText.IsNull();
    }
}
