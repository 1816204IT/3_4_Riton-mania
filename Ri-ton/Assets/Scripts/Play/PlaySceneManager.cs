using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using Ritonmania;

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

/// <summary>
/// プレイシーン管理クラス
/// </summary>
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
    private const float hexagonAnimSpeed = 0.5f;

    private NCMB.HighScore highScore;

    [SerializeField]
    private GameObject tutorialCanvas = null;
    private bool isTutorialEnd = false;

    private bool isPlayEnd = false;
    private bool isAchieveHishScore = false;    // ハイスコアを達成したか

    private NCMB.HighScore prePlayHighScore;

    [SerializeField]
    private GameObject bloomCubes = null;
    [SerializeField]
    private GameObject hexagons = null;

    private void Start()
    {
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        jsonManager = GameObject.FindGameObjectWithTag("JsonManager").GetComponent<JsonManager>();
        NullCheck();

        musicName.text = SelectedMap.instance.musicName;
        difficultyName.text = SelectedMap.instance.difficultyName;

        // カーソルの表示をOFFにする
        Cursor.visible = false;
        // 六角形のアニメーションを再開する
        PlayHexAnim();
        // チュートリアル表示
        tutorialCanvas.SetActive(true);

        // ハイスコアクラスのインスタンス
        string name = FindObjectOfType<UserAuth>().playerName;
        highScore = new NCMB.HighScore(name, 0);

        // 前回のハイスコア
        prePlayHighScore = new NCMB.HighScore(name, 0);
        prePlayHighScore.Fetch();

        // 背景の選択
        if (UserPreference.instance.isBloomCubes)
        {
            UserPreference.instance.isBloomCubes = false;
            bloomCubes.SetActive(true);
            hexagons.SetActive(false);
        }
        else
        {
            UserPreference.instance.isBloomCubes = true;
            bloomCubes.SetActive(false);
            hexagons.SetActive(true);
        }
    }

    void Update()
    {
        // チュートリアル終了検知
        if (isTutorialEnd == false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isTutorialEnd = true;
                playStartTimer.isTutorialEnd = true;
                tutorialCanvas.SetActive(false);
            }
        }

        if (isTutorialEnd == false)
        {
            return;
        }

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

        // 曲が終了したらリザルトデータをサーバーに送信する
        // 曲が終わった瞬間にaudioSource.time = 0になるので曲の長さの0.1秒前に終了させることで安全に判定する
        if (musicPlayer.audioSource.time + 0.1f >= musicPlayer.audioSource.clip.length)
        {
            musicPlayer.audioSource.Stop();
            SaveResultData();
        }

        // プレイが終わったか
        if (isPlayEnd)
        {
            // ハイスコア達成か
            if (isAchieveHishScore)
            {
                // サーバーにリザルトデータの送信が完了したか
                if (highScore.fetchState == FetchState.succeeded)
                {
                    SceneManager.sceneLoaded += ResultSceneLoaded;
                    SceneManager.LoadScene("Result");
                }
            }
            else
            {
                SceneManager.sceneLoaded += ResultSceneLoaded;
                SceneManager.LoadScene("Result");
            }
        }

        // デバッグ用　Rキーでリザルト画面へ遷移
        if (Input.GetKeyDown(KeyCode.R))
        {
            SaveResultData();
        }

        // デバッグ用、曲を指定時間までスキップする
        if (Input.GetKeyDown(KeyCode.T))
        {
            musicPlayer.audioSource.time = 80.0f;
        }
    }

    void SaveResultData()
    {
        highScore.score = scoreCounter.GetScore();
        highScore.combo = comboCounter.maxCombo;
        highScore.acc = (int)(accCounter.acc * 100);      // accの100倍を代入

        RankNumber rankNum;
        if (highScore.score >= 1000000)
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

        highScore.rank = (int)rankNum;


        // ハイスコア更新したかどうか
        if (highScore.score > prePlayHighScore.score)
        {
            // ハイスコアとしてNCMBに登録
            highScore.Save();
            // ハイスコア達成フラグを立てる
            isAchieveHishScore = true;
        }

        // プレイ終了フラグを立てる
        isPlayEnd = true;
    }

    // リザルトシーンの変数[resultDataInput]に値をセットする
    private void ResultSceneLoaded(Scene next,LoadSceneMode mode)
    {
        ResultDataInput resultDataInput = GameObject.FindGameObjectWithTag("ResultDataInput").GetComponent<ResultDataInput>();
        // データの作成
        ResultShowData data = new ResultShowData();
        data.score = highScore.score;
        data.acc = highScore.acc;
        data.maxCombo = accCounter.totalNoteNum;
        data.combo = highScore.combo;
        data.perfectNum = accCounter.totalPerfectNum;
        data.goodNum = accCounter.totalGoodNum;
        data.missNum = accCounter.totalMissNum;
        data.rankImageNum = highScore.rank;
        data.isAciveHighScore = isAchieveHishScore;
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
            moveMaskMat.SetFloat("_MoveSpeed", 0.0f);
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
            moveMaskMat.SetFloat("_MoveSpeed", hexagonAnimSpeed);
        }
        else
        {
            Debug.LogError("shaderのパラメーター名が存在しません");
        }
    }

    public void Retry()
    {
        PlayHexAnim();
        SceneManager.LoadScene("Play");
    }

    public void Quit()
    {
        PlayHexAnim();
        SceneManager.LoadScene(TitleSceneManager.prevSceneName);
    }

    private void NullCheck()
    {
        musicPlayer.IsNull(nameof(musicPlayer));
        menuObj.IsNull(nameof(menuObj));
        jsonManager.IsNull(nameof(jsonManager));
        playStartTimer.IsNull(nameof(playStartTimer));
        accCounter.IsNull(nameof(accCounter));
        comboCounter.IsNull(nameof(comboCounter));
        scoreCounter.IsNull(nameof(scoreCounter));
        musicName.IsNull(nameof(musicName));
        difficultyName.IsNull(nameof(difficultyName));
        moveMaskMat.IsNull(nameof(moveMaskMat));
        tutorialCanvas.IsNull(nameof(tutorialCanvas));
        bloomCubes.IsNull(nameof(bloomCubes));
        hexagons.IsNull(nameof(hexagons));
    }
}
