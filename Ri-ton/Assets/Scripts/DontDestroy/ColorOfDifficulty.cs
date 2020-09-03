using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorOfDifficulty : MonoBehaviour
{
    [System.Serializable]
    public struct DifficultyColor
    {
        public Color easy;
        public Color normal;
        public Color hard;
        public Color expert;
    }

    [SerializeField]
    private DifficultyColor difficultyColor = default;

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

    public Color GetColorOfDifficulty()
    {
        string difficultyName = SelectedMap._instance._difficultyName;

        if (difficultyName == "Easy")
        {
            return difficultyColor.easy;
        }
        else if (difficultyName == "Normal")
        {
            return difficultyColor.normal;
        }
        else if (difficultyName == "Hard")
        {
            return difficultyColor.hard;
        }
        else if (difficultyName == "Expert")
        {
            return difficultyColor.expert;
        }

        Debug.Log("無効な難易度名が設定されています");
        return Color.black;
    }

    //シングルトン実態を返す
    public static ColorOfDifficulty _instance
    {
        get;
        private set;
    }
}
