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

    public static UserPreference instance { get; private set; }
    public bool isBloomCubes { get; set; } = true;  // プレイ背景がキューブの方か
    public float noteSpeedNum { get; private set; } = c_init_note_speed;    // 1.0f～10.0f(0.5刻み)
    public float offsetValueNum { get; private set; } = 0.0f;       // -10.0f～10.0f(1.0刻み)
    public float musicVolume { get; set; } = c_init_music_volume;   // 0.0f～1.0f(0.05刻み)
    public float seVolume { get; set; } = c_init_se_volume;         // 0.0f～1.0f(0.05刻み)
    public bool isTutorial { get; set; }

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
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
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
        noteSpeedNum = settingNum;
    }

    public void ChangeOffsetValue(float offsetNum)
    {
        offsetValueNum = offsetNum;
    }

    public void NoteSpeedUp()
    {
        noteSpeedNum = (noteSpeedNum <= 9.5f) ? noteSpeedNum + 0.5f : noteSpeedNum;
    }

    public void NoteSpeedDown()
    {
        noteSpeedNum = (noteSpeedNum > 1.0f) ? noteSpeedNum - 0.5f : noteSpeedNum;
    }

    public void AddOffset()
    {
        offsetValueNum = (offsetValueNum < 100.0f) ? offsetValueNum + 2 : offsetValueNum;
    }

    public void SubtractOffset()
    {
        offsetValueNum = (offsetValueNum > -100.0f) ? offsetValueNum - 2 : offsetValueNum;
    }

    public float NoteSpeed()
    {
        return noteSpeedUnit * noteSpeedNum;
    }

    public float UserOffset()
    {
        return offsetValueUnit * offsetValueNum;
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
        data.noteSpeedNum = (int)(noteSpeedNum * 10);
        data.offsetValueNum = (int)offsetValueNum;
        data.musicVolume = (int)(musicVolume * 100);
        data.seVolume = (int)(seVolume * 100);
        data.isTutorial = isTutorial;
        return data;
    }

    private void Convert(Ritonmania.LocalUserData data)
    {
        userAuth.playerName = data.playerName;
        userAuth.password = data.password;
        noteSpeedNum = data.noteSpeedNum / 10.0f;
        offsetValueNum = (float)data.offsetValueNum;
        musicVolume = data.musicVolume / 100.0f;
        seVolume = data.seVolume / 100.0f;
        isTutorial = data.isTutorial;
    }

    private void NullCheck()
    {
        userAuth.IsNull();
    }
}