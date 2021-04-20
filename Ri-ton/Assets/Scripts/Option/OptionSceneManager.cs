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

    /// <summary>
    /// ノーツ落下速度アップ
    /// </summary>
    public void NoteSpeedUp()
    {
        UserPreference.Instance.NoteSpeedUp();
    }

    /// <summary>
    /// ノーツ落下速度ダウン
    /// </summary>
    public void NoteSpeedDown()
    {
        UserPreference.Instance.NoteSpeedDown();
    }

    /// <summary>
    /// 譜面オフセットプラス
    /// </summary>
    public void AddOffset()
    {
        UserPreference.Instance.AddOffset();
        offsetText.text = UserPreference.Instance.UserOffset().ToString();
    }

    /// <summary>
    /// 譜面オフセットマイナス
    /// </summary>
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
