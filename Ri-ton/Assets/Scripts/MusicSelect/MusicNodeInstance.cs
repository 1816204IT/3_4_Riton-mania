using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 曲パネル生成クラス
/// </summary>
public class MusicNodeInstance : MonoBehaviour
{
    public List<GameObject> NodeGameObjectList { get; } = new List<GameObject>();
    public List<RectTransform> NodeRectTransformList { get; } = new List<RectTransform>();

    [SerializeField]
    private GameObject prefab = null;

    void Awake()
    {
        PrefabInstance();
    }

    /// <summary>
    /// プレハブからインスタンスを生成する
    /// </summary>
    private void PrefabInstance()
    {
        for(int i = 0; i < MusicInfoList.Instance.MusicNum(); i++)
        {
            NodeGameObjectList.Add(Instantiate(prefab, this.transform, false));
            NodeRectTransformList.Add(NodeGameObjectList[i].GetComponent<RectTransform>());
        }

        for(int i = 0; i < MusicInfoList.Instance.MusicNum(); i++)
        {
            MusicNode musicNode = NodeGameObjectList[i].GetComponent<MusicNode>();
            musicNode.MyNodeNum = i;
            musicNode.SetMusicNameText();
            musicNode.SetJacketImage();
        }
    }
}