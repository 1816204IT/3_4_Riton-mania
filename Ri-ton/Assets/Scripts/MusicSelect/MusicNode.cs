using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 曲パネルクラス
/// </summary>
public class MusicNode : MonoBehaviour
{
    public int MyNodeNum { get; set; } = -1;

    [SerializeField]
    private Button button = null;
    [SerializeField]
    private Text musicNameText = null;
    [SerializeField]
    private Image jacketImage = null;
    [SerializeField]
    private MusicsInfo musicsInfo = null;

    private MusicNodeScroll musicNodeScroll = null;
    private BigNodeInformation bigNode = null;

    private void Start()
    {
        musicNodeScroll = this.transform.parent.gameObject.GetComponent<MusicNodeScroll>();
        bigNode = GameObject.FindGameObjectWithTag("BigNode").GetComponent<BigNodeInformation>();
        NullCheck();

        button.onClick.AddListener(OnClickNode);
    }

    /// <summary>
    /// 曲パネルを選択した際の処理
    /// </summary>
    void OnClickNode()
    {
        musicNodeScroll.SelectedNodeChangesFunc(MyNodeNum, musicsInfo.Info[MyNodeNum].musicName);
    }

    /// <summary>
    /// 曲名を設定する
    /// </summary>
    public void SetMusicNameText()
    {
        musicNameText.text = musicsInfo.Info[MyNodeNum].musicName;
    }

    /// <summary>
    /// 曲の画像を設定する
    /// </summary>
    public void SetJacketImage()
    {
        jacketImage.sprite = musicsInfo.Info[MyNodeNum].bgImage;
    }

    private void NullCheck()
    {
        musicNodeScroll.IsNull();
        button.IsNull();
        musicNameText.IsNull();
        bigNode.IsNull();
        jacketImage.IsNull();
        musicsInfo.IsNull();
    }
}