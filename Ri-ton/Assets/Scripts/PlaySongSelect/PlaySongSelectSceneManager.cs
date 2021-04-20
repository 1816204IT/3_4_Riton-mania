using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 曲選択画面管理クラス
/// </summary>
public class PlaySongSelectSceneManager : MonoBehaviour
{
    [SerializeField]
    private Text playerName = null;
    [SerializeField]
    private GameObject settingCanvas = null;
    [SerializeField]
    private Image myCharacter = null;
    [SerializeField]
    private GameObject tutorialCanvas = null;

    private void Start()
    {
        TitleSceneManager.prevSceneName = "PlaySongSelect";
        NullCheck();

        playerName.text = FindObjectOfType<UserAuth>().playerName;

        // カーソルの表示をONにする
        Cursor.visible = true;

        // キャラクター表示
        int charaNum = UserPreference.Instance.GetCharacterNumber();
        myCharacter.sprite = CharacterInfoList.instance.GetSprite(charaNum);
    }

    void Update()
    {
        if (tutorialCanvas.activeSelf == true)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingCanvas.activeSelf)
            {
                settingCanvas.SetActive(false);
                return;
            }
            SceneManager.LoadScene("Title");
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            OnClickPlayButton();
        }
    }

    /// <summary>
    /// PLAYボタン押下時にプレイシーンへ遷移する
    /// </summary>
    public void OnClickPlayButton()
    {
        SceneManager.LoadScene("Play");
    }

    /// <summary>
    /// EDITボタン押下時に曲編集シーンへ遷移する
    /// </summary>
    public void OnClickEditButton()
    {
        SceneManager.LoadScene("Edit");
    }

    /// <summary>
    /// 戻るボタン押下時に前のシーンへ遷移する
    /// </summary>
    public void OnClickBackButton()
    {
        if (settingCanvas.activeSelf)
        {
            settingCanvas.SetActive(false);
            // ユーザー設定をローカルデータとして保存
            UserPreference.Instance.Save();
            return;
        }
        SceneManager.LoadScene("Title");
    }

    /// <summary>
    /// 設定ボタン押下時に設定画面を開く
    /// </summary>
    public void OnClickSettingButton()
    {
        if (settingCanvas.activeSelf == false)
        {
            settingCanvas.SetActive(true);
        }
    }

    private void NullCheck()
    {
        playerName.IsNull();
        settingCanvas.IsNull();
        myCharacter.IsNull();
        tutorialCanvas.IsNull();
    }
}