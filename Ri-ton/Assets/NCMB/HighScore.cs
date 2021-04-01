using NCMB;
using System.Collections.Generic;
using Ritonmania;

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

        public FetchState fetchState { get; private set; } = FetchState.non;

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
            string className = SelectedMap.instance.GetMusicEnglishName() + "_" + SelectedMap.instance.difficultyName;
            NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(className);
            query.WhereEqualTo("Name", name);
            query.FindAsync((NCMBQueryCallback<NCMBObject>)((List<NCMBObject> objList, NCMBException e) =>
            {
                // 検索成功したら
                if (e == null)
                {
                    // ハイスコアが未登録だったら
                    if (objList.Count == 0)
                    {
                        NCMBObject obj  = new NCMBObject(className);
                        obj["Name"]     = name;
                        obj["Score"]    = score;
                        obj["Combo"]    = combo;
                        obj["Acc"]      = acc;
                        obj["Rank"]     = rank;
                        obj.SaveAsync();
                    }
                    // ハイスコアが登録済みだったら
                    else
                    {
                        objList[0]["Score"] = score;
                        objList[0]["Combo"] = combo;
                        objList[0]["Acc"]   = acc;
                        objList[0]["Rank"]  = rank;
                        objList[0].SaveAsync();
                    }

                    this.fetchState = FetchState.succeeded;
                }
                else
                {
                    this.fetchState = FetchState.succeeded;
                }
            }));
        }

        // サーバーからハイスコアを取得
        public void Fetch()
        {
            fetchState = FetchState.trying;

            // データストアの「HighScore」クラスから、Nameをキーにして検索
            string className = SelectedMap.instance.GetMusicEnglishName() + "_" + SelectedMap.instance.difficultyName;
            NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(className);
            query.WhereEqualTo("Name", name);
            query.FindAsync((NCMBQueryCallback<NCMBObject>)((List<NCMBObject> objList, NCMBException e) =>
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

                    this.fetchState = FetchState.succeeded;
                }
                else
                {
                    this.fetchState = FetchState.failed;
                }
            }));
        }
    }
}
