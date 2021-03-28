using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自己ベストとオンラインランキングを切り替える
/// </summary>
public class ScoreViewSwitcher : MonoBehaviour
{
    [SerializeField]
    private GameObject rankingView = null;
    [SerializeField]
    private GameObject myBestView = null;
    [SerializeField]
    private GameObject rankingTextObj = null;
    [SerializeField]
    private GameObject myBestTextObj = null;
    [SerializeField]
    private GameObject noDataTextObj = null;

    // 自身のハイスコア表示中か(falseならランキング表示中)
    bool isModeHighScore = false;

    void Start()
    {
        if (rankingView == null || myBestView == null || rankingTextObj == null
            || myBestTextObj == null || noDataTextObj == null)
        {
            Debug.Log("nullを検知");
        }

        Init();
    }

    private void Init()
    {
        if (isModeHighScore)
        {
            rankingView.SetActive(false);
            rankingTextObj.SetActive(false);
            myBestView.SetActive(true);
            myBestTextObj.SetActive(true);
            noDataTextObj.SetActive(true);
        }
        else
        {
            rankingView.SetActive(true);
            rankingTextObj.SetActive(true);
            myBestView.SetActive(false);
            myBestTextObj.SetActive(false);
            noDataTextObj.SetActive(false);
        }
    }

    public void ToggleViewMode()
    {
        isModeHighScore = !isModeHighScore;
        Init();
    }
}