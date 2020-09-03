using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankImageList : MonoBehaviour
{ 
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

    public Sprite GetSmallSprite(int characterNum)
    {
        return sprites[characterNum].smallSprite;
    }

    //シングルトン実態を返す
    public static RankImageList _instance
    {
        get;
        private set;
    }
}

