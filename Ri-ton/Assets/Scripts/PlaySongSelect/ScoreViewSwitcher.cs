using UnityEngine;

/// <summary>
/// 自己ベストとオンラインランキングの表示を切り替える
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

    private bool isModeHighScore = false;  // 自身のハイスコア表示中か(falseならランキング表示中)

    void Start()
    {
        NullCheck();
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

    /// <summary>
    /// 表示を切り替える
    /// </summary>
    public void ToggleViewMode()
    {
        isModeHighScore = !isModeHighScore;
        Init();
    }

    private void NullCheck()
    {
        rankingView.IsNull();
        myBestView.IsNull();
        rankingTextObj.IsNull();
        myBestTextObj.IsNull();
        noDataTextObj.IsNull();
    }
}