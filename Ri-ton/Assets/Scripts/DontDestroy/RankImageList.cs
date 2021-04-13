using UnityEngine;

/// <summary>
/// ランク画像（S,A,B,C,D）の情報を持ったクラス
/// </summary>
public class RankImageList : MonoBehaviour
{
    public static RankImageList Instance { get; private set; }

    [System.Serializable]
    struct CharacterSprite
    {
        public Sprite sprite;
        public Sprite smallSprite;
    }

    [SerializeField]
    private CharacterSprite[] sprites;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public Sprite GetSprite(int characterNum)
    {
        return sprites[characterNum].sprite;
    }

    public Sprite GetSmallSprite(int characterNum)
    {
        return sprites[characterNum].smallSprite;
    }
}