using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public enum RankNumber
{ 
    SS,
    S,
    A,
    B,
    C,
    D,
    MAX
}

public class PlaySceneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject menuObj = null;
    [SerializeField]
    private PlayStartTimer playStartTimer = null;
    [SerializeField]
    private AccCounter accCounter = null;
    [SerializeField]
    private ComboCounter comboCounter = null;
    [SerializeField]
    private ScoreCounter scoreCounter = null;
    [SerializeField]
    private Text musicName = null;
    [SerializeField]
    private Text difficultyName = null;

    private MusicPlayer musicPlayer = null;
    private JsonManager jsonManager = null;

    [SerializeField]
    private Material moveMaskMat = null;
    private int hexagonAnimSpeed = -1;

    private NCMB.HighScore highScore;

    private void Start()
    {
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        jsonManager = GameObject.FindGameObjectWithTag("JsonManager").GetComponent<JsonManager>();

        if (musicPlayer == null || menuObj == null || jsonManager == null
             || playStartTimer == null || accCounter == null || comboCounter == null
             || scoreCounter == null || musicName == null || difficultyName == null
             || moveMaskMat == null)
        {
            Debug.Log("nullを検知");
        }

        musicName.text = SelectedMap._instance._musicName;
        difficultyName.text = SelectedMap._instance._difficultyName;

        // カーソルの表示をOFFにする
        Cursor.visible = false;
        // 六角形のアニメーションを再開する
        PlayHexAnim();
        // 初めて遊ぶ曲の場合にサーバーに初期データを作成する
        string name = FindObjectOfType<UserAuth>()._playerName;
        highScore = new NCMB.HighScore(name, 0);
        highScore.CreateInitialData();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            musicPlayer.PlayPause();
            playStartTimer.enabled = false;
            menuObj.SetActive(true);

            // カーソルの表示をONにする
            Cursor.visible = true;
            // 六角形のアニメーションを止める
            PoseHexAnim();
        }

        // 曲が終了したらリザルト画面へ遷移する
        // 曲が終わった瞬間にaudioSource.time = 0になるので曲の長さの0.1秒前に終了させることで安全に判定する
        if (musicPlayer._audioSource.time + 0.1f >= musicPlayer._audioSource.clip.length)
        {
            SaveResultData();
            SceneManager.sceneLoaded += ResultSceneLoaded;
            SceneManager.LoadScene("Result");
        }

        // デバッグ用　Rキーでリザルト画面へ遷移
        if (Input.GetKeyDown(KeyCode.R))
        {
            SaveResultData();
            SceneManager.sceneLoaded += ResultSceneLoaded;
            SceneManager.LoadScene("Result");
        }
    }

    void SaveResultData()
    {
        highScore.score = scoreCounter._score;
        highScore.combo = comboCounter._maxCombo;
        highScore.acc = (int)(accCounter._acc * 100);      // accの100倍を代入

        RankNumber rankNum;
        if (highScore.score >= 999900)
        {
            rankNum = RankNumber.SS;
        }
        else if (highScore.score >= 950000)
        {
            rankNum = RankNumber.S;
        }
        else if (highScore.score >= 900000)
        {
            rankNum = RankNumber.A;
        }
        else if (highScore.score >= 850000)
        {
            rankNum = RankNumber.B;
        }
        else if (highScore.score >= 800000)
        {
            rankNum = RankNumber.C;
        }
        else
        {
            rankNum = RankNumber.D;
        }

        // ハイスコアとしてNCMBに登録
        highScore.rank = (int)rankNum;
        highScore.Save();
    }

    // リザルトシーンの変数[resultDataInput]に値をセットする
    private void ResultSceneLoaded(Scene next,LoadSceneMode mode)
    {
        ResultDataInput resultDataInput = GameObject.FindGameObjectWithTag("ResultDataInput").GetComponent<ResultDataInput>();
        // データの作成
        ResultShowData data = new ResultShowData();
        data.score = highScore.score;
        data.acc = highScore.acc;
        data.maxCombo = accCounter._totalNoteNum;
        data.combo = highScore.combo;
        data.perfectNum = accCounter._totalPerfectNum;
        data.goodNum = accCounter._totalGoodNum;
        data.missNum = accCounter._totalMissNum;
        data.rankImageNum = highScore.rank;
        resultDataInput.SetResultShowData(data);
        SceneManager.sceneLoaded -= ResultSceneLoaded;
    }

    public void Continue()
    {
        playStartTimer.enabled = true;
        playStartTimer.TimerReset();
        menuObj.SetActive(false);

        // カーソルの表示をOFFにする
        Cursor.visible = false;
        // 六角形のアニメーションを再開する
        PlayHexAnim();
    }

    // 六角形のアニメーションを停止する
    private void PoseHexAnim()
    {
        if (moveMaskMat.HasProperty("_MoveSpeed"))
        {
            hexagonAnimSpeed = moveMaskMat.GetInt("_MoveSpeed");
            moveMaskMat.SetInt("_MoveSpeed", 0);
        }
        else
        {
            Debug.Log("shaderのパラメーター名が存在しません");
        }
    }

    // 六角形のアニメーションを再開する
    private void PlayHexAnim()
    {
        if (moveMaskMat.HasProperty("_MoveSpeed"))
        {
            moveMaskMat.SetInt("_MoveSpeed", hexagonAnimSpeed);
        }
        else
        {
            Debug.Log("shaderのパラメーター名が存在しません");
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene("Play");
    }

    public void Quit()
    {
        SceneManager.LoadScene(TitleSceneManager._prevSceneName);
    }
}
