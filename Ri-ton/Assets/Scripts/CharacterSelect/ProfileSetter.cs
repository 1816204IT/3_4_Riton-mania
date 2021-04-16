using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// キャラクタープロフィール情報設定クラス
/// </summary>
public class ProfileSetter : MonoBehaviour
{
    [SerializeField]
    private Text height = null;
    [SerializeField]
    private Text age = null;
    [SerializeField]
    private Text birthDay = null;
    [SerializeField]
    private Text personality = null;
    [SerializeField]
    private Text likes = null;
    [SerializeField]
    private Text unLikes = null;
    [SerializeField]
    private Text illustrator = null;

    void Start()
    {
        NullCheck();
    }

    /// <summary>
    /// プロフィール情報を設定する
    /// </summary>
    /// <param name="data">プロフィールデータ</param>
    public void UpdateProfile(Ritonmania.CharacterData data)
    {
        height.text         = data.height;
        age.text            = data.age;
        birthDay.text       = data.birthDay;
        personality.text    = data.personality;
        likes.text          = data.likes;
        unLikes.text        = data.unLikes;
        illustrator.text    = data.illustrator;
    }

    private void NullCheck()
    {
        age.IsNull();
        birthDay.IsNull();
        likes.IsNull();
        illustrator.IsNull();
    }
}
