using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 曲パネルクラス
/// </summary>
public class MusicNode : MonoBehaviour
{
    private int myNodeNum = -1;
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

        if (musicNodeScroll == null || button == null || musicNameText == null
            || bigNode == null || jacketImage == null)
        {
            Debug.Log("nullを検知");
        }

        button.onClick.AddListener(OnClickNode);
    }

    void OnClickNode()
    {
        musicNodeScroll.SelectedNodeChangesFunc(myNodeNum, MusicInfoList._instance.GetMusicName(myNodeNum));
    }

    public void SetMusicNameText()
    {
        musicNameText.text = MusicInfoList._instance.GetMusicName(myNodeNum);
    }

    public void SetJacketImage()
    {
        jacketImage.sprite = MusicInfoList._instance.GetBgImage(myNodeNum);
    }

    public int _myNodeNum
    {
        set { myNodeNum = value; }
    }
}
