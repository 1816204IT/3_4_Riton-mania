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

    private MusicNodeScroll musicNodeScroll = null;
    private BigNodeInformation bigNode = null;

    private void Start()
    {
        musicNodeScroll = this.transform.parent.gameObject.GetComponent<MusicNodeScroll>();
        bigNode = GameObject.FindGameObjectWithTag("BigNode").GetComponent<BigNodeInformation>();
        NullCheck();

        button.onClick.AddListener(OnClickNode);
    }

    void OnClickNode()
    {
        musicNodeScroll.SelectedNodeChangesFunc(MyNodeNum, MusicInfoList.Instance.GetMusicName(MyNodeNum));
    }

    public void SetMusicNameText()
    {
        musicNameText.text = MusicInfoList.Instance.GetMusicName(MyNodeNum);
    }

    public void SetJacketImage()
    {
        jacketImage.sprite = MusicInfoList.Instance.GetBgImage(MyNodeNum);
    }

    private void NullCheck()
    {
        musicNodeScroll.IsNull();
        button.IsNull();
        musicNameText.IsNull();
        bigNode.IsNull();
        jacketImage.IsNull();
    }
}