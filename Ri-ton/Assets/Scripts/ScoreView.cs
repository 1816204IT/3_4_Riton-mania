using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// オンラインランキングビューを表示する
/// </summary>
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

    private bool isScoreFetched;
    private bool isRankFetched;
    private bool isLeaderBoardFetched;

    private const float reFetchRankersTime = 3.0f;  // ランカー0人の時に再度サーバーにフェッチするまでの時間
    private int rankersNum = -1;
    private float noRankersTimer = 0.0f;

    void Start()
    {
        NullCheck();
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
        // ランカー0人なら一定時間後に再度フェッチする
        if (rankersNum == 0)
        {
            noRankersTimer += Time.deltaTime;
            if (noRankersTimer >= reFetchRankersTime)
            {
                noRankersTimer = 0.0f;
                UpdateResultData();
            }
        }

        // 現在のハイスコアの取得が完了したら1度だけ実行
        if ( (highScore.score != -1) && (isScoreFetched == false) )
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
        if ( (lBoard.currentRank != 0) && (isRankFetched == false) )
        {
            lBoard.FetchTopRankers();
            isRankFetched = true;
        }

        // ランキングの取得が完了したら1度だけ実行
        if (lBoard.IsIconFetchEnd() && (lBoard.topRankers != null) && (isLeaderBoardFetched == false) )
        {
            // 取得したトップランキングを表示
            topRankers = lBoard.GetTopRankers();
            topRankersIcon = lBoard.GetTopRankersIcon();
            isLeaderBoardFetched = true;

            // 取得したランカーの人数を保存
            rankersNum = topRankers.Count;

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
        topRankers.Clear();
        topRankersIcon.Clear();

        for (int i = 0; i < nodeObjList.Count; i++)
        {
            Destroy(nodeObjList[i]);
        }
        Destroy(highScoreNode);
        nodeObjList.Clear();
        noDataText.text = "";
        rankersNum = 0;

        // データ取得開始
        DoUpdate();
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
        //プレイヤ名を設定
        node.transform.Find("PlayerNameText").GetComponent<Text>().text = highScore.name;
        //スコアを設定
        PathBuilder.Clear();
        PathBuilder.AppendFormat("score : {0}", highScore.score.ToString("N0"));
        node.transform.Find("ScoreText").GetComponent<Text>().text = PathBuilder.ToString();
        //最大コンボ数を設定
        PathBuilder.Clear();
        PathBuilder.AppendFormat("({0}{1})", highScore.combo.ToString("N0"), "x");
        node.transform.Find("ComboText").GetComponent<Text>().text = PathBuilder.ToString();
        //精度を設定
        PathBuilder.Clear();
        PathBuilder.AppendFormat("{0}%", (highScore.acc / 100.0f).ToString("f2"));
        node.transform.Find("AccText").GetComponent<Text>().text = PathBuilder.ToString();
        //アイコン画像を設定
        node.transform.Find("PlayerImage").GetComponent<Image>().sprite = CharacterInfoList.instance.GetIconSprite(UserPreference.instance._characterNum);
        //ランク画像を設定
        node.transform.Find("RankImage").GetComponent<Image>().sprite = RankImageList.instance.GetSmallSprite(highScore.rank);
        //順位を設定
        node.transform.Find("RankNumber").GetComponent<Text>().text = "";
    }

    private void SetRankingNode(GameObject node, int i)
    {
        NCMB.HighScore rankers = topRankers[i];

        //プレイヤ名を設定
        node.transform.Find("PlayerNameText").GetComponent<Text>().text = rankers.name;
        //スコアを設定
        PathBuilder.Clear();
        PathBuilder.AppendFormat("score : {0}", rankers.score.ToString("N0"));
        node.transform.Find("ScoreText").GetComponent<Text>().text = PathBuilder.ToString();
        //最大コンボ数を設定
        PathBuilder.Clear();
        PathBuilder.AppendFormat("({0}{1})", rankers.combo.ToString("N0"), "x");
        node.transform.Find("ComboText").GetComponent<Text>().text = PathBuilder.ToString();
        //精度を設定
        PathBuilder.Clear();
        PathBuilder.AppendFormat("{0}%", (rankers.acc / 100.0f).ToString("f2"));
        node.transform.Find("AccText").GetComponent<Text>().text = PathBuilder.ToString();

        //アイコン画像を設定
        node.transform.Find("PlayerImage").GetComponent<Image>().sprite = CharacterInfoList.instance.GetIconSprite(topRankersIcon[i].character);

        //ランク画像を設定
        node.transform.Find("RankImage").GetComponent<Image>().sprite = RankImageList.instance.GetSmallSprite(rankers.rank);
        //順位を設定
        node.transform.Find("RankNumber").GetComponent<Text>().text = (i + 1).ToString();
    }

    private void NullCheck()
    {
        nodePrefab.IsNull(nameof(nodePrefab));
        rankingContent.IsNull(nameof(rankingContent));
        noDataText.IsNull(nameof(noDataText));
    }
}