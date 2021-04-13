using UnityEngine;
using Ritonmania;

namespace Ritonmania
{
    [System.Serializable]
    public class LocalUserData
    {
        // ※データ保存の際に整数値にする必要があるため10～100倍します。
        // 例 0.15は100倍して15にしてから保存。値として使用する時は100で割って0.15に戻す。
        public string playerName;
        public string password;
        public int noteSpeedNum;    // 10～100(/10して使用する)
        public int offsetValueNum;  // -10～10(等倍で使用する)
        public int musicVolume;     // 0～100(/100して使用する)
        public int seVolume;        // 0～100(/100して使用する)
        public bool isTutorial;     // チュートリアルが完了しているか

        public LocalUserData()
        {
            playerName = null;
            password = null;
            noteSpeedNum = 50;
            offsetValueNum = 0;
            musicVolume = 50;
            seVolume = 50;
            isTutorial = false;
        }
    }
}

/// <summary>
/// ノーツ速度、ユーザーオフセット、カーソルカラーなどの設定
/// </summary>
public class UserPreference : MonoBehaviour
{
    private const float c_init_note_speed = 1.0f;
    private const float c_init_music_volume = 0.5f;
    private const float c_init_se_volume = 0.5f;

    public static UserPreference Instance { get; private set; }
    public bool IsBloomCubes { get; set; } = true;  // プレイ背景がキューブの方か
    public float NoteSpeedNum { get; private set; } = c_init_note_speed;    // 1.0f～10.0f(0.5刻み)
    public float OffsetValueNum { get; private set; } = 0.0f;       // -10.0f～10.0f(1.0刻み)
    public float MusicVolume { get; set; } = c_init_music_volume;   // 0.0f～1.0f(0.05刻み)
    public float SeVolume { get; set; } = c_init_se_volume;         // 0.0f～1.0f(0.05刻み)
    public bool IsTutorial { get; set; }

    private const int c_max_note_speed = 4000;    // ノーツ速度の最大値
    private const int c_min_note_speed = 400;     // ノーツ速度の最小値
    private const int c_max_lane_num = 4;
    private const float c_max_offset_value = 0.04f;
    private const float c_min_offset_value = -0.04f;
    private const float c_note_size_x = 150.0f;

    private float noteSpeedUnit;
    private float offsetValueUnit;
    private NCMB.CharacterIcon characterIcon = new NCMB.CharacterIcon(null);
    private UserAuth userAuth = null;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        userAuth = FindObjectOfType<UserAuth>();
        NullCheck();

        noteSpeedUnit = (c_max_note_speed - c_min_note_speed) / 10.0f;
        offsetValueUnit = (c_max_offset_value - c_min_offset_value) / 10.0f;
    }

    // 現在のユーザー設定をローカルファイルに保存する
    public void Save()
    {
        JsonManager jsonManager;
        jsonManager = GameObject.FindGameObjectWithTag("JsonManager").GetComponent<JsonManager>();
        jsonManager.SaveUserPreference(GetCovertData());
    }

    // ローカルファイルのユーザーデータを今のデータに反映する
    // ゲームを起動した際に一度行う
    // この時に playerName != null なら自動ログインする
    // @rtnparam false = ユーザー未登録
    public bool Load()
    {
        JsonManager jsonManager;
        jsonManager = GameObject.FindGameObjectWithTag("JsonManager").GetComponent<JsonManager>();
        var data = jsonManager.LoadUserPreference();
        Convert(data);
        // 過去にログインしている場合
        if (userAuth.playerName != "")
        {
            userAuth.logIn(userAuth.playerName, userAuth.password);
            characterIcon.name = userAuth.playerName;
            CharacterIconFetch();
            return true;
        }
        return false;
    }

    public void CharacterIconFetch()
    {
        // キャラクター番号を取得
        characterIcon.Fetch();
    }

    public void characterIconLogOut()
    {
        characterIcon = new NCMB.CharacterIcon(name);
    }

    // サーバーのキャラクター番号を現在のキャラクターと同期する
    public void AsyncCharacterIcon()
    {
        characterIcon.Save();
    }

    public void ChangeNoteSpeed(float settingNum)
    {
        NoteSpeedNum = settingNum;
    }

    public void ChangeOffsetValue(float offsetNum)
    {
        OffsetValueNum = offsetNum;
    }

    public void NoteSpeedUp()
    {
        NoteSpeedNum = (NoteSpeedNum <= 9.5f) ? NoteSpeedNum + 0.5f : NoteSpeedNum;
    }

    public void NoteSpeedDown()
    {
        NoteSpeedNum = (NoteSpeedNum > 1.0f) ? NoteSpeedNum - 0.5f : NoteSpeedNum;
    }

    public void AddOffset()
    {
        OffsetValueNum = (OffsetValueNum < 100.0f) ? OffsetValueNum + 2 : OffsetValueNum;
    }

    public void SubtractOffset()
    {
        OffsetValueNum = (OffsetValueNum > -100.0f) ? OffsetValueNum - 2 : OffsetValueNum;
    }

    public float NoteSpeed()
    {
        return noteSpeedUnit * NoteSpeedNum;
    }

    public float UserOffset()
    {
        return offsetValueUnit * OffsetValueNum;
    }

    public float NoteSizeX()
    {
        return c_note_size_x;
    }

    public int GetCharacterNumber()
    {
        return characterIcon.character;
    }

    public void SetCharacterNumber(int num)
    {
        characterIcon.character = num;
    }

    public void SetCharacterIconName(string name)
    {
        characterIcon.name = name;
    }

    public FetchState GetIconFetchState()
    {
        return characterIcon._iconFetchState;
    }

    // 0レーン目のノーツX座標
    public float NotePosXOfLaneZero()
    {
        return -(c_note_size_x * (c_max_lane_num / 2)) + (c_note_size_x / 2);
    }

    private Ritonmania.LocalUserData GetCovertData()
    {
        Ritonmania.LocalUserData data = new Ritonmania.LocalUserData();
        data.playerName = userAuth.playerName;
        data.password = userAuth.password;
        data.noteSpeedNum = (int)(NoteSpeedNum * 10);
        data.offsetValueNum = (int)OffsetValueNum;
        data.musicVolume = (int)(MusicVolume * 100);
        data.seVolume = (int)(SeVolume * 100);
        data.isTutorial = IsTutorial;
        return data;
    }

    private void Convert(Ritonmania.LocalUserData data)
    {
        userAuth.playerName = data.playerName;
        userAuth.password = data.password;
        NoteSpeedNum = data.noteSpeedNum / 10.0f;
        OffsetValueNum = (float)data.offsetValueNum;
        MusicVolume = data.musicVolume / 100.0f;
        SeVolume = data.seVolume / 100.0f;
        IsTutorial = data.isTutorial;
    }

    private void NullCheck()
    {
        userAuth.IsNull();
    }
}