using NCMB;
using System.Collections.Generic;

public class LeaderBoard
{
    public int currentRank { get; private set; } = 0;
    public List<NCMB.HighScore> topRankers { get; private set; } = null;

    private const int c_viewRankingNumMax = 10; // 何人までランキングに表示するか
    private int viewRankingNum = 0; // 何人表示するか
    private List<NCMB.CharacterIcon> topRankersIcon = null;

    // 現プレイヤーのハイスコアを受けとってランクを取得 ---------------
    public void FetchRank(int currentScore)
    {
        // データスコアの「HighScore」から検索
        string className = SelectedMap.instance.GetMusicEnglishName() + "_" + SelectedMap.instance.difficultyName;
        NCMBQuery<NCMBObject> rankQuery = new NCMBQuery<NCMBObject>(className);
        rankQuery.WhereGreaterThan("Score", currentScore);
        rankQuery.CountAsync((int count, NCMBException e) => {

            if (e != null)
            {
                //件数取得失敗
            }
            else
            {
                //件数取得成功
                currentRank = count + 1; // 自分よりスコアが上の人がn人いたら自分はn+1位
            }
        });
    }

    // サーバーからトップランカーを取得 ---------------    
    public void FetchTopRankers()
    {
        topRankers = null;
        topRankersIcon = null;
        viewRankingNum = 0;

        // データストアの「HighScore」クラスから検索
        string className = SelectedMap.instance.GetMusicEnglishName() + "_" + SelectedMap.instance.difficultyName;
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(className);
        query.OrderByDescending("Score");
        query.Limit = c_viewRankingNumMax; // 何人まで取得するか
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {

            if (e != null)
            {
                //検索失敗時の処理
            }
            else
            {
                //検索成功時の処理
                List<NCMB.HighScore> list = new List<NCMB.HighScore>();
                List<NCMB.CharacterIcon> iconList = new List<CharacterIcon>();
                // 取得したレコードをHighScoreクラスとして保存
                foreach (NCMBObject obj in objList)
                {
                    viewRankingNum++;

                    // 名前の取得
                    string name = System.Convert.ToString(obj["Name"]);

                    // ハイスコアの取得
                    int score = System.Convert.ToInt32(obj["Score"]);
                    int combo = System.Convert.ToInt32(obj["Combo"]);
                    int acc = System.Convert.ToInt32(obj["Acc"]);
                    int rank = System.Convert.ToInt32(obj["Rank"]);
                    list.Add(new HighScore(name, score, combo, acc, rank));

                    // アイコン画像の取得-------
                    CharacterIcon icon = new CharacterIcon(name);
                    icon.Fetch();
                    iconList.Add(icon);
                }
                topRankers = list;
                topRankersIcon = iconList;
            }
        });
    }

    public List<NCMB.HighScore> GetTopRankers()
    {
        return topRankers;
    }

    public List<NCMB.CharacterIcon> GetTopRankersIcon()
    {
        return topRankersIcon;
    }

    // トップランカーのアイコン取得が完了しているか
    public bool IsIconFetchEnd()
    {
        if (topRankersIcon == null)
        {
            return false;
        }

        int fetchEndNum = 0;
        foreach (NCMB.CharacterIcon icon in topRankersIcon)
        {
            if (icon.character != 5)
            {
                fetchEndNum++;
            }
        }

        if (fetchEndNum < viewRankingNum)
        {
            return false;
        }

        return true;
    }
}
