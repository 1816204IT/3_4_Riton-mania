using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// オプション画面管理クラス
/// </summary>
public class OptionSceneManager : MonoBehaviour
{
    [SerializeField]
    private Text offsetText = null;

    void Awake()
    {
        NullCheck();
        SelectedMap.instance.musicName = "castle";
        SelectedMap.instance.difficultyName = "Easy";
        offsetText.text = UserPreference.instance.UserOffset().ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Title");
        }
    }

    public void NoteSpeedUp()
    {
        UserPreference.instance.NoteSpeedUp();
    }

    public void NoteSpeedDown()
    {
        UserPreference.instance.NoteSpeedDown();
    }

    public void AddOffset()
    {
        UserPreference.instance.AddOffset();
        offsetText.text = UserPreference.instance.UserOffset().ToString();
    }

    public void SubtractOffset()
    {
        UserPreference.instance.SubtractOffset();
        offsetText.text = UserPreference.instance.UserOffset().ToString();
    }

    private void NullCheck()
    {
        offsetText.IsNull(nameof(offsetText));
    }
}
