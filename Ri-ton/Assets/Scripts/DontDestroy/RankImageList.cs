using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ランク画像（S,A,B,C,D）の情報を持ったクラス
/// </summary>
public class RankImageList : MonoBehaviour
{
    public static RankImageList instance { get; private set; }

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
        return sprites[characterNum].sprite;
    }

    public Sprite GetSmallSprite(int characterNum)
    {
        return sprites[characterNum].smallSprite;
    }
}