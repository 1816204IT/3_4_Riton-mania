using UnityEngine;

/// <summary>
/// ランク画像（S,A,B,C,D）の情報を持ったクラス
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/RankImages")]
public class RankImages : ScriptableObject
{
    [System.Serializable]
    public struct RankImage
    {
        [field: SerializeField]
        public Sprite sprite { get; private set; }
        [field: SerializeField]
        public Sprite smallSprite { get; private set; }
    }

    [field: SerializeField]
    public RankImage[] images { get; private set; }
}