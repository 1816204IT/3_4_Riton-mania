using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ヘッダー情報表示クラス
/// プレイヤ名を表示する
/// </summary>
public class HeaderInfo : MonoBehaviour
{
    [SerializeField]
    private Text playerNameText = null;
    [SerializeField]
    private CharactersInfo charactersInfo = null;

    void Start()
    {
        NullCheck();
        SetPlayerNameColor();
    }

    private void Update()
    {
        string playerName = FindObjectOfType<UserAuth>().playerName;
        NullCheck();
        playerNameText.text = playerName;
    }

    /// <summary>
    /// プレイヤ名の色を変更する
    /// </summary>
    public void SetPlayerNameColor()
    {
        int characterNum = UserPreference.Instance.GetCharacterNumber();
        Color headerNameColor = Color.white;
        if (CharacterIsAlreadySelected(characterNum))
        {
            headerNameColor = charactersInfo.Info[characterNum].color;
        }
        
        playerNameText.color = headerNameColor;
    }

    /// <summary>
    /// キャラクター選択済みか
    /// ログインして最初にキャラクター選択画面を開いた場合はキャラクター未選択状態となるためチェックが必要
    /// </summary>
    private bool CharacterIsAlreadySelected(int characterNum)
    {
        return (characterNum >= 0 && characterNum <= 4);
    }

    private void NullCheck()
    {
	    playerNameText.IsNull();
        charactersInfo.IsNull();
    }
}
