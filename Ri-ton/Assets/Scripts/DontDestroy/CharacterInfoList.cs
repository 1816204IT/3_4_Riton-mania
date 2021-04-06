using UnityEngine;

/// <summary>
/// 以下の情報を持ったクラス
/// キャラクターの立ち絵画像、アイコン画像、色
/// </summary>
public class CharacterInfoList : MonoBehaviour
{
    [System.Serializable]
    struct CharacterSprite
    {
        public Sprite sprite;
        public Sprite iconSprite;
        public Color color;
    }

    [SerializeField]
    private CharacterSprite[] infos;

    public static CharacterInfoList instance { get; private set; }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public Sprite GetSprite(int characterNum)
    {
        return infos[characterNum].sprite;
    }

    public Sprite GetIconSprite(int characterNum)
    {
        return infos[characterNum].iconSprite;
    }

    public Color GetColor(int characterNum)
    {
        if (characterNum >= infos.Length)
        {
            return Color.white;
        }

        return infos[characterNum].color;
    }
}
