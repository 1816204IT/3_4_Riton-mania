using UnityEngine;

/// <summary>
/// 曲名、BG、音声ファイルを保管
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/MusicsInfo")]
public class MusicsInfo : ScriptableObject
{
    [System.Serializable]
    public struct MusicInfo
    {
        [field: SerializeField]
        public string musicName { get; private set; }
        [field: SerializeField]
        public string musicEnglishName { get; private set; }
        [field: SerializeField]
        public Sprite bgImage { get; private set; }
        [field: SerializeField]
        public AudioClip audioClip { get; private set; }
    }

    [field: SerializeField]
    public MusicInfo[] Info { get; private set; }
}
