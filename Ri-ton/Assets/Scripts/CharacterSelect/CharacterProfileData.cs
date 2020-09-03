using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ritonmania
{
    [System.Serializable]
    public struct CharacterData
    {
        public string name;
        public Color color;
        public int age;
        public int birthDay;    // 6月19日なら0619
        public string like;
        public string illustrator;
    }
}

public class CharacterProfileData : MonoBehaviour
{


    [SerializeField]
    private Ritonmania.CharacterData[] characterDatas;

    public Ritonmania.CharacterData GetCharacterData(int index)
    {
        return characterDatas[index];
    }
}
