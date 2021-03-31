using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;

namespace Ritonmania
{
    [System.Serializable]
    public class LocalUserData
    {
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
    public static UserPreference instance { get; private set; }
    public bool isBloomCubes { get; set; } = true;  // プレイ背景がキューブの方か
    public float noteSpeedNum { get; private set; } = 1.0f;     // 1.0f～10.0f(0.5刻み)
    public float offsetValueNum { get; private set; } = 0.0f;   // -10.0f～10.0f(1.0刻み)
    public float musicVolume { get; set; } = 0.5f;  // 0.0f～1.0f(0.05刻み)
    public float seVolume { get; set; } = 0.5f;     // 0.0f～1.0f(0.05刻み)
    public bool isTutorial { get; set; }

    private const int max_note_speed = 4000;    // ノーツ速度の最大値
    private const int min_note_speed = 400;     // ノーツ速度の最小値
    private float noteSpeedUnit;      

    private const float max_offset_value = 0.04f;
    private const float min_offset_value = -0.04f;
    private float offsetValueUnit;

    private NCMB.CharacterIcon characterIcon = new NCMB.CharacterIcon(null);

    private const float note_size_x = 150.0f;
    private const int max_lane_num = 4;

    UserAuth userAuth = null;

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

        noteSpeedUnit = (max_note_speed - min_note_speed) / 10.0f;
        offsetValueUnit = (max_offset_value - min_offset_value) / 10.0f;
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
        if (userAuth._playerName != "")
        {
            userAuth.logIn(userAuth._playerName, userAuth._password);
            characterIcon.name = userAuth._playerName;
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

    public float _noteSpeed
    {
        get { return noteSpeedUnit * noteSpeedNum; }
    }

    public float _userOffset
    {
        get { return offsetValueUnit * offsetValueNum; }
    }

    public float _note_size_x
    {
        get { return note_size_x; }
    }

    public int _characterNum
    {
        get { return characterIcon.character; }
        set { characterIcon.character = value; }
    }

    public string _characterIconName
    {
        set { characterIcon.name = value; }
    }

    public FetchState _iconFetchState
    { 
        get { return characterIcon._iconFetchState; }
    }

    // 0レーン目のノーツX座標
    public float _notePosXOfLaneZero
    {
        get { return -(note_size_x * (max_lane_num / 2)) + (note_size_x / 2); }
    }

    private Ritonmania.LocalUserData GetCovertData()
    {
        Ritonmania.LocalUserData data = new Ritonmania.LocalUserData();
        data.playerName = userAuth._playerName;
        data.password = userAuth._password;
        data.noteSpeedNum = (int)(noteSpeedNum * 10);
        data.offsetValueNum = (int)offsetValueNum;
        data.musicVolume = (int)(musicVolume * 100);
        data.seVolume = (int)(seVolume * 100);
        data.isTutorial = isTutorial;
        return data;
    }

    private void Convert(Ritonmania.LocalUserData data)
    {
        userAuth._playerName = data.playerName;
        userAuth._password = data.password;
        noteSpeedNum = data.noteSpeedNum / 10.0f;
        offsetValueNum = (float)data.offsetValueNum;
        musicVolume = data.musicVolume / 100.0f;
        seVolume = data.seVolume / 100.0f;
        isTutorial = data.isTutorial;
    }

    private void NullCheck()
    {
        userAuth.IsNull(nameof(userAuth));
    }
}