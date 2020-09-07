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

//ノーツ速度、ユーザーオフセット、カーソルカラーなどの設定
//シングルトンクラス
public class UserPreference : MonoBehaviour
{
    private const int max_note_speed = 4000;    // ノーツ速度の最大値
    private const int min_note_speed = 400;     // ノーツ速度の最小値
    private float noteSpeedUnit;
    private float noteSpeedNum = 1.0f;          // 1.0f～10.0f(0.5刻み)

    private const float max_offset_value = 0.04f;
    private const float min_offset_value = -0.04f;
    private float offsetValueUnit;
    private float offsetValueNum = 0.0f;        // -10.0f～10.0f(1.0刻み)

    private float musicVolume = 0.5f;           // 0.0f～1.0f(0.05刻み)
    private float seVolume = 0.5f;              // 0.0f～1.0f(0.05刻み)

    private bool isTutorial = false;

    private NCMB.CharacterIcon characterIcon = new NCMB.CharacterIcon(null);

    private const float note_size_x = 150.0f;
    private const int max_lane_num = 4;

    UserAuth userAuth = null;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);

        userAuth = FindObjectOfType<UserAuth>();
        if (userAuth == null)
        {
            Debug.Log("nullを検知");
        }

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

    public void NotesSpeedUp()
    {
        noteSpeedNum = (noteSpeedNum <= 9.5f) ? noteSpeedNum + 0.5f : noteSpeedNum;
    }

    public void NotesSpeedDown()
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

    ///====================以下プロパティ====================

    public float _notesSpeed
    {
        get { return noteSpeedUnit * noteSpeedNum; }
    }

    public float _noteSpeedNum
    {
        get { return noteSpeedNum; }
        set { noteSpeedNum = value; }
    }

    public float _userOffset
    {
        get { return offsetValueUnit * offsetValueNum; }
    }

    public float _offsetValueNum
    {
        get { return offsetValueNum; }
        set { offsetValueNum = value; }
    }

    public float _musicVolume
    {
        get { return musicVolume; }
        set { musicVolume = value; }
    }

    public float _seVolume
    {
        get { return seVolume; }
        set { seVolume = value; }
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

    public IconFetchState _iconFetchState
    { 
        get { return characterIcon._iconFetchState; }
    }

    public bool _isTutorial
    { 
        get { return isTutorial; }
        set { isTutorial = value; }
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
        data.noteSpeedNum = (int)(_noteSpeedNum * 10);
        data.offsetValueNum = (int)_offsetValueNum;
        data.musicVolume = (int)(_musicVolume * 100);
        data.seVolume = (int)(_seVolume * 100);
        data.isTutorial = isTutorial;
        return data;
    }

    private void Convert(Ritonmania.LocalUserData data)
    {
        userAuth._playerName = data.playerName;
        userAuth._password = data.password;
        _noteSpeedNum = data.noteSpeedNum / 10.0f;
        _offsetValueNum = (float)data.offsetValueNum;
        _musicVolume = data.musicVolume / 100.0f;
        _seVolume = data.seVolume / 100.0f;
        _isTutorial = data.isTutorial;
    }

    //シングルトン実態を返す
    public static UserPreference _instance
    {
        get;
        private set;
    }
}