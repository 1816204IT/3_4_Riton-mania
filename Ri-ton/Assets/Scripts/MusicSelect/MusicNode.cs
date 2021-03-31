using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 曲パネルクラス
/// </summary>
public class MusicNode : MonoBehaviour
{
    public int myNodeNum { get; set; } = -1;
    private MusicNodeScroll musicNodeScroll = null;
    private BigNodeInformation bigNode = null;
    [SerializeField]
    private Button button = null;
    [SerializeField]
    private Text musicNameText = null;
    [SerializeField]
    private Image jacketImage = null;

    private void Start()
    {
        musicNodeScroll = this.transform.parent.gameObject.GetComponent<MusicNodeScroll>();
        bigNode = GameObject.FindGameObjectWithTag("BigNode").GetComponent<BigNodeInformation>();
        NullCheck();

        button.onClick.AddListener(OnClickNode);
    }

    void OnClickNode()
    {
        musicNodeScroll.SelectedNodeChangesFunc(myNodeNum, MusicInfoList.instance.GetMusicName(myNodeNum));
    }

    public void SetMusicNameText()
    {
        musicNameText.text = MusicInfoList.instance.GetMusicName(myNodeNum);
    }

    public void SetJacketImage()
    {
        jacketImage.sprite = MusicInfoList.instance.GetBgImage(myNodeNum);
    }

    private void NullCheck()
    {
        musicNodeScroll.IsNull(nameof(musicNodeScroll));
        button.IsNull(nameof(button));
        musicNameText.IsNull(nameof(musicNameText));
        bigNode.IsNull(nameof(bigNode));
        jacketImage.IsNull(nameof(jacketImage));
    }
}