using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ホールド中ノーツの色情報を持ったクラス
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/NoteColor")]
public class NoteColor : ScriptableObject
{
    [field: SerializeField]
    public Color even { get; private set; } // 偶数レーンノーツの色
    [field: SerializeField]
    public Color odd { get; private set; }  // 奇数レーンノーツの色
    [field: SerializeField]
    public Color evenLongDefault { get; private set; }  // 偶数レーンロングノーツの通常色
    [field: SerializeField]
    public Color evenLongHolding { get; private set; }  // 偶数レーンロングノーツのホールド中の色
    [field: SerializeField]
    public Color oddLongDefault { get; private set; }  // 奇数レーンロングノーツの通常色
    [field: SerializeField]
    public Color oddLongHolding { get; private set; }  // 奇数レーンロングノーツのホールド中の色
}
