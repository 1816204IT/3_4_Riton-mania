using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        return infos[characterNum].sprite;
    }

    public Sprite GetIconSprite(int characterNum)
    {
        return infos[characterNum].iconSprite;
    }

    public Color GetColor(int characterNum)
    {
        return infos[characterNum].color;
    }

    //シングルトン実態を返す
    public static CharacterInfoList _instance
    {
        get;
        private set;
    }
}
