using NCMB;
using System.Collections.Generic;

// ユーザーIDをキーにしてハイスコアを取得する
namespace NCMB
{
    public class HighScore
    {
        public string name { get; set; }
        public int score { get; set; }
        public int combo { get; set; }
        public int acc { get; set; }
        public int rank { get; set; }

        private FetchState fetchState = FetchState.non;

        // コンストラクタ -----------------------------------
        public HighScore(string _name, int _score, int _combo, int _acc, int _rank)
        {
            name        = _name;
            score       = _score;
            combo       = _combo;
            acc         = _acc;
            rank        = _rank;
        }
        // コンストラクタ(初期化用) -----------------------------------
        public HighScore(string _name, int _score)
        {
            name = _name;
            score = _score;
        }

        // サーバーにハイスコアを保存 -----------------------------------
        public void Save()
        {
            fetchState = FetchState.trying;

            // データストアの「HighScore」クラスから、Nameをキーにして検索
            string className = SelectedMap._instance.GetMusicEnglishName() + "_" + SelectedMap._instance._difficultyName;
            NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(className);
            query.WhereEqualTo("Name", name);
            query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
            {
                // 検索成功したら
                if (e == null)
                {
                    objList[0]["Score"] = score;
                    objList[0]["Combo"] = combo;
                    objList[0]["Acc"] = acc;
                    objList[0]["Rank"] = rank;
                    objList[0].SaveAsync();

                    fetchState = FetchState.succeeded;
                }
                else
                {
                    fetchState = FetchState.failed;
                }
            });
        }

        // サーバーからハイスコアを取得
        public void Fetch()
        {
            fetchState = FetchState.trying;

            // データストアの「HighScore」クラスから、Nameをキーにして検索
            string className = SelectedMap._instance.GetMusicEnglishName() + "_" + SelectedMap._instance._difficultyName;
            NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(className);
            query.WhereEqualTo("Name", name);
            query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
            {
                // 検索成功したら
                if (e == null)
                {
                    // ハイスコアが未登録だったら
                    if (objList.Count == 0)
                    {
                        score = 0;
                    }
                    // ハイスコアが登録済みだったら
                    else
                    {
                        score = System.Convert.ToInt32(objList[0]["Score"]);
                        combo = System.Convert.ToInt32(objList[0]["Combo"]);
                        acc = System.Convert.ToInt32(objList[0]["Acc"]);
                        rank = System.Convert.ToInt32(objList[0]["Rank"]);
                    }

                    fetchState = FetchState.succeeded;
                }
                else
                {
                    fetchState = FetchState.failed;
                }
            });
        }

        // 初めてプレイする曲の場合に初期データを作成する
        public void CreateInitialData()
        {
            fetchState = FetchState.trying;

            // データストアの「HighScore」クラスから、Nameをキーにして検索
            string className = SelectedMap._instance.GetMusicEnglishName() + "_" + SelectedMap._instance._difficultyName;
            NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(className);
            query.WhereEqualTo("Name", name);
            query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
            {
                // 検索成功したら
                if (e == null)
                {
                    // ハイスコアが未登録だったら
                    if (objList.Count == 0)
                    {
                        NCMBObject obj = new NCMBObject(className);
                        obj["Name"] = name;
                        obj["Score"] = 0;
                        obj["Combo"] = 0;
                        obj["Acc"] = 0;
                        obj["Rank"] = 0;
                        obj.SaveAsync();
                        score = 0;
                    }
                    // ハイスコアが登録済みだったら
                    else
                    {
                        //何もしない
                    }

                    fetchState = FetchState.succeeded;
                }
                else
                {
                    fetchState = FetchState.failed;
                }
            });
        }

        public FetchState _fetchState
        { 
            get { return fetchState; }
        }
    }
}
