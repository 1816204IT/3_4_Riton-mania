using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ritonmania
{
    [System.Serializable]
    public struct CharacterData
    {
        public string name;         // 名前
        public string height;       // 身長
        public string age;          // 年齢
        public string birthDay;     // 誕生日
        public string personality;  // 性格
        public string likes;        // 好きなもの
        public string unLikes;      // 嫌いなもの
        public string illustrator;  // 作者
    }
}

/// <summary>
/// キャラクタープロフィール情報
/// </summary>
public class CharacterProfileData : MonoBehaviour
{
    [SerializeField]
    private Ritonmania.CharacterData[] characterDatas;

    public Ritonmania.CharacterData GetCharacterData(int index)
    {
        return characterDatas[index];
    }
}
