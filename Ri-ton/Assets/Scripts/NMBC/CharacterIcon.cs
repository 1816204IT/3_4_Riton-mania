using System.Collections.Generic;
using Ritonmania;

namespace Ritonmania
{
    public enum FetchState
    {
        non,
        trying,
        succeeded,
        failed
    }
}

namespace NCMB
{  
    /// <summary>
    /// ユーザーIDをキーにしてキャラクター番号を取得する 
    /// </summary>
    public class CharacterIcon
    {
        public string name { get; set; }
        public int character { get; set; }

        private FetchState fetchState = FetchState.non;

        public CharacterIcon(string _name)
        {
            name        = _name;
            character = 5;  // デフォルトでは画像未選択状態
        }

        /// <summary>
        /// サーバーにキャラクター番号を保存する
        /// </summary>
        public void Save()
        {
            // データストアの「CharacterIcon」クラスから、Nameをキーにして検索
            NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("CharacterIcon");
            query.WhereEqualTo("Name", name);
            query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
            {
                // 検索成功したら
                if (e == null)
                {
                    objList[0]["Character"] = character;
                    objList[0].SaveAsync();
                }
            });
        }

        /// <summary>
        /// サーバーからキャラクター番号を取得
        /// </summary>
        public void Fetch()
        {
            fetchState = FetchState.trying;

            // データストアの「CharacterIcon」クラスから、Nameをキーにして検索
            NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("CharacterIcon");
            query.WhereEqualTo("Name", name);
            query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
            {
                // 検索成功したら
                if (e == null)
                {
                    // キャラクター番号が未登録だったら
                    if (objList.Count == 0)
                    {
                        NCMBObject obj = new NCMBObject("CharacterIcon");
                        obj["Name"] = name;
                        obj["Character"] = character;
                        obj.SaveAsync();
                    }
                    // キャラクター番号が登録済みだったら
                    else
                    {
                        character = System.Convert.ToInt32(objList[0]["Character"]);
                    }

                    fetchState = FetchState.succeeded;
                }
                else
                {
                    fetchState = FetchState.failed;
                }
            });
        }

        /// <summary>
        /// フェッチ状況を取得する
        /// </summary>
        public FetchState _iconFetchState
        { 
            get { return fetchState; }
        }
    }
}