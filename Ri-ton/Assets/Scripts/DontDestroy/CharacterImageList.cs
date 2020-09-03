using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterImageList : MonoBehaviour
{
    [System.Serializable]
    struct CharacterSprite
    {
        public Sprite sprite;
        public Sprite iconSprite;
    }

    [SerializeField]
    private CharacterSprite[] sprites;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public Sprite GetSprite(int characterNum)
    {
        return sprites[characterNum].sprite;
    }

    public Sprite GetIconSprite(int characterNum)
    {
        return sprites[characterNum].iconSprite;
    }

    //シングルトン実態を返す
    public static CharacterImageList _instance
    {
        get;
        private set;
    }
}
