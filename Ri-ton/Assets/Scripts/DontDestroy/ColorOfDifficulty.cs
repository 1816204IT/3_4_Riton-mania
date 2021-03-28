using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 難易度に対応する色情報を持ったクラス
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/ColorOfDifficulty")]
public class ColorOfDifficulty : ScriptableObject
{
    [field: SerializeField]
    public Color easy { get; private set; }
    [field: SerializeField]
    public Color normal { get; private set; }
    [field: SerializeField]
    public Color hard { get; private set; }
    [field: SerializeField]
    public Color expert { get; private set; }

    public Color GetColorOfDifficulty(string difficultyName)
    {
        if (difficultyName == "Easy")
        {
            return easy;
        }
        else if (difficultyName == "Normal")
        {
            return normal;
        }
        else if (difficultyName == "Hard")
        {
            return hard;
        }
        else if (difficultyName == "Expert")
        {
            return expert;
        }

        Debug.LogError("無効な難易度名です");
        return Color.black;
    }
}