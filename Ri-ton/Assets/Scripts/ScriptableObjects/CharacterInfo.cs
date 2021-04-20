using UnityEngine;

/// <summary>
/// 以下の情報を持ったクラス
/// キャラクターの立ち絵画像、アイコン画像、色
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/CharacterInfo")]
public class CharacterInfo : ScriptableObject
{
    [field: SerializeField]
    public Sprite sprite { get; private set; }
    [field: SerializeField]
    public Sprite iconSprite { get; private set; }
    [field: SerializeField]
    public Color color { get; private set; }
}
