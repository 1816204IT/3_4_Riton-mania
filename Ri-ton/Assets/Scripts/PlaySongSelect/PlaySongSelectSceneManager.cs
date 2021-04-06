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
        int charaNum = UserPreference.instance.GetCharacterNumber();
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

    public void OnClickPlayButton()
    {
        SceneManager.LoadScene("Play");
    }

    public void OnClickEditButton()
    {
        SceneManager.LoadScene("Edit");
    }

    public void OnClickBackButton()
    {
        if (settingCanvas.activeSelf)
        {
            settingCanvas.SetActive(false);
            // ユーザー設定をローカルデータとして保存
            UserPreference.instance.Save();
            return;
        }
        SceneManager.LoadScene("Title");
    }

    public void OnClickSettingButton()
    {
        if (settingCanvas.activeSelf == false)
        {
            settingCanvas.SetActive(true);
        }
    }

    private void NullCheck()
    {
        playerName.IsNull(nameof(playerName));
        settingCanvas.IsNull(nameof(settingCanvas));
        myCharacter.IsNull(nameof(myCharacter));
        tutorialCanvas.IsNull(nameof(tutorialCanvas));
    }
}