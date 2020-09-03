using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour
{
    [SerializeField]
    private GameObject nodePrefab = null;
    [SerializeField]
    private GameObject rankingContent = null;
    [SerializeField]
    private GameObject myBestContent = null;
    [SerializeField]
    private Text noDataText = null;

    private List<GameObject> nodeObjList = new List<GameObject>();
    StringBuilder PathBuilder;

    private LeaderBoard lBoard;
    private NCMB.HighScore highScore;
    public List<NCMB.HighScore> topRankers = new List<NCMB.HighScore>();
    public List<NCMB.CharacterIcon> topRankersIcon = new List<NCMB.CharacterIcon>();
    private GameObject highScoreNode = null;

    bool isScoreFetched;
    bool isRankFetched;
    bool isLeaderBoardFetched;

    private float timer = 0;
    private const float updateWaitTime = 0.3f;

    void Start()
    {
        if (nodePrefab == null || rankingContent == null || noDataText == null)
        {
            Debug.Log("nullを検知");
        }

        PathBuilder = new StringBuilder();

        lBoard = new LeaderBoard();

        // フラグ初期化
        isRankFetched = false;
        isLeaderBoardFetched = false;

        string name = FindObjectOfType<UserAuth>()._playerName;
        highScore = new NCMB.HighScore(name, -1);
    }

    public void Update()
    {
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0.0f)
            {
                timer = 0.0f;
                DoUpdate();
            }
        }

        // 現在のハイスコアの取得が完了したら1度だけ実行
        if (highScore.score != -1 && !isScoreFetched)
        {
            lBoard.FetchRank(highScore.score);
            isScoreFetched = true;

            // ハイスコアがある場合
            if (highScore.score != 0)
            {
                highScoreNode = Instantiate(nodePrefab);
                highScoreNode.transform.SetParent(myBestContent.transform);
                SetHighScoreNode(highScoreNode);
            }
            // ハイスコアがない場合
            else
            {
                noDataText.text = "No Data";
            }
        }

        // 現在の順位の取得が完了したら1度だけ実行
        if (lBoard.currentRank != 0 && !isRankFetched)
        {
            lBoard.FetchTopRankers();
            isRankFetched = true;
        }

        // ランキングの取得が完了したら1度だけ実行
        if (lBoard.IsIconFetchEnd() && (lBoard.topRankers != null) && !isLeaderBoardFetched)
        {
            // 取得したトップランキングを表示
            topRankers = lBoard.GetTopRankers();
            topRankersIcon = lBoard.GetTopRankersIcon();
            isLeaderBoardFetched = true;

            for(int i = 0; i < topRankers.Count; i++)
            {
                var obj = Instantiate(nodePrefab);
                obj.transform.SetParent(rankingContent.transform);
                //ノード情報を更新
                SetRankingNode(obj, i);
                nodeObjList.Add(obj);
            }
        }
    }

    public void UpdateResultData()
    {
        // タイマーが0になったら更新をかける
        // 短い時間に連続でランキング更新をするとサーバーとの連携の兼ね合いで
        // ランキングが更新されないことがあるので一定時間待ってから更新する
        timer = updateWaitTime;

        topRankers.Clear();
        topRankersIcon.Clear();

        for (int i = 0; i < nodeObjList.Count; i++)
        {
            Destroy(nodeObjList[i]);
        }
        Destroy(highScoreNode);
        nodeObjList.Clear();
        noDataText.text = "";
    }

    private void DoUpdate()
    {
        // ハイスコアを取得
        string name = FindObjectOfType<UserAuth>()._playerName;
        highScore = new NCMB.HighScore(name, -1);
        highScore.Fetch();
        // フラグ初期化
        isScoreFetched = false;
        isRankFetched = false;
        isLeaderBoardFetched = false;
    }

    private void SetHighScoreNode(GameObject node)
    {
        PathBuilder.Clear();

        PathBuilder.Clear();
        //プレイヤ名を設定
        node.transform.Find("PlayerNameText").GetComponent<Text>().text = highScore.name;
        //スコアと最大コンボ数を設定
        PathBuilder.AppendFormat("score : {0} [{1}x]", highScore.score, highScore.combo);
        node.transform.Find("ScoreText").GetComponent<Text>().text = PathBuilder.ToString();
        //精度を設定
        PathBuilder.Clear();
        PathBuilder.AppendFormat("{0}%", (highScore.acc / 100.0f).ToString("f2"));
        node.transform.Find("AccText").GetComponent<Text>().text = PathBuilder.ToString();
        //アイコン画像を設定
        node.transform.Find("PlayerImage").GetComponent<Image>().sprite = CharacterImageList._instance.GetIconSprite(UserPreference._instance._characterNum);
        //ランク画像を設定
        node.transform.Find("RankImage").GetComponent<Image>().sprite = RankImageList._instance.GetSmallSprite(highScore.rank);
        //順位を設定
        //node.transform.Find("RankNumber").GetComponent<Text>().text = lBoard.currentRank.ToString();
        node.transform.Find("RankNumber").GetComponent<Text>().text = "";
    }

    private void SetRankingNode(GameObject node, int i)
    {
        NCMB.HighScore rankers = topRankers[i];

        PathBuilder.Clear();
        //プレイヤ名を設定
        node.transform.Find("PlayerNameText").GetComponent<Text>().text = rankers.name;
        //スコアと最大コンボ数を設定
        PathBuilder.AppendFormat("score : {0} [{1}x]", rankers.score, rankers.combo);
        node.transform.Find("ScoreText").GetComponent<Text>().text = PathBuilder.ToString();
        //精度を設定
        PathBuilder.Clear();
        PathBuilder.AppendFormat("{0}%", (rankers.acc / 100.0f).ToString("f2"));
        node.transform.Find("AccText").GetComponent<Text>().text = PathBuilder.ToString();

        //アイコン画像を設定
        node.transform.Find("PlayerImage").GetComponent<Image>().sprite = CharacterImageList._instance.GetIconSprite(topRankersIcon[i].character);

        //ランク画像を設定
        node.transform.Find("RankImage").GetComponent<Image>().sprite = RankImageList._instance.GetSmallSprite(rankers.rank);
        //順位を設定
        node.transform.Find("RankNumber").GetComponent<Text>().text = (i + 1).ToString();
    }
}
