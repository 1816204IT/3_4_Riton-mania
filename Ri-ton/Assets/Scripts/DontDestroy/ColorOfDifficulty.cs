using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 難易度に対応する色情報を持ったクラス
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/ColorOfDifficulty")]
public class ColorOfDifficulty : ScriptableObject
{
    [SerializeField]
    private Color easy;
    [SerializeField]
    private Color normal;
    [SerializeField]
    private Color hard;
    [SerializeField]
    private Color expert;

    public Color GetColorOfDifficulty(string difficultyName)
    {
        if (difficultyName == "Easy")
        {
            return Easy;
        }
        else if (difficultyName == "Normal")
        {
            return Normal;
        }
        else if (difficultyName == "Hard")
        {
            return Hard;
        }
        else if (difficultyName == "Expert")
        {
            return Expert;
        }

        Debug.LogError("無効な難易度名が設定されています");
        return Color.black;
    }

    public Color Easy
    { 
        get { return easy; }
    }

    public Color Normal
    {
        get { return normal; }
    }

    public Color Hard
    {
        get { return hard; }
    }

    public Color Expert
    {
        get { return expert; }
    }
}