using UnityEngine;

/// <summary>
/// 難易度に対応する色情報を持ったクラス
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/ColorOfDifficulty")]
public class DifficultyColor : ScriptableObject
{
    [field: SerializeField]
    public Color Easy { get; private set; }
    [field: SerializeField]
    public Color Normal { get; private set; }
    [field: SerializeField]
    public Color Hard { get; private set; }
    [field: SerializeField]
    public Color Expert { get; private set; }

    public Color GetDifficultyColor(string difficultyName)
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

        Debug.LogError("無効な難易度名です");
        return Color.white;
    }
}