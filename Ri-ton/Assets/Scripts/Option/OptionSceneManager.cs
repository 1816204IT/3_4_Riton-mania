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
        SelectedMap.Instance.MusicName = "castle";
        SelectedMap.Instance.DifficultyName = "Easy";
        offsetText.text = UserPreference.Instance.UserOffset().ToString();
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
        UserPreference.Instance.NoteSpeedUp();
    }

    public void NoteSpeedDown()
    {
        UserPreference.Instance.NoteSpeedDown();
    }

    public void AddOffset()
    {
        UserPreference.Instance.AddOffset();
        offsetText.text = UserPreference.Instance.UserOffset().ToString();
    }

    public void SubtractOffset()
    {
        UserPreference.Instance.SubtractOffset();
        offsetText.text = UserPreference.Instance.UserOffset().ToString();
    }

    private void NullCheck()
    {
        offsetText.IsNull();
    }
}
