using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// リザルトシーンにて今回のスコアが全体の何位かを検索する
/// </summary>
public class MyRanking : MonoBehaviour
{
    private LeaderBoard lBoard = new LeaderBoard();
    bool isRankFetched = false;

    [SerializeField]
    private ResultDataInput resultDataInput = null;
    [SerializeField]
    private Text myRankingText = null;

    void Start()
    {
        if (resultDataInput == null || myRankingText == null)
        {
            Debug.Log("nullを検知");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isRankFetched == false)
        {
            lBoard.FetchRank(resultDataInput.GetScore());
        }

        // 現在の順位の取得が完了したら1度だけ実行
        if (lBoard.currentRank != 0 && (isRankFetched == false))
        {
            myRankingText.text = lBoard.currentRank.ToString();
            isRankFetched = true;

            // 1位なら文字色を黄色にする
            if (lBoard.currentRank == 1)
            {
                myRankingText.color = Color.yellow;
            }
        }
    }
}