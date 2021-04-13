using UnityEngine;

/// <summary>
/// ホールド中ノーツの色情報を持ったクラス
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/NoteColor")]
public class NoteColor : ScriptableObject
{
    [field: SerializeField]
    public Color Even { get; private set; } // 偶数レーンノーツの色
    [field: SerializeField]
    public Color Odd { get; private set; }  // 奇数レーンノーツの色
    [field: SerializeField]
    public Color EvenLongDefault { get; private set; }  // 偶数レーンロングノーツの通常色
    [field: SerializeField]
    public Color EvenLongHolding { get; private set; }  // 偶数レーンロングノーツのホールド中の色
    [field: SerializeField]
    public Color OddLongDefault { get; private set; }  // 奇数レーンロングノーツの通常色
    [field: SerializeField]
    public Color OddLongHolding { get; private set; }  // 奇数レーンロングノーツのホールド中の色
}
