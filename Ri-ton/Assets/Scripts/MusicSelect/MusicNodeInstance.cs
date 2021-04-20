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
    [SerializeField]
    private MusicsInfo musicsInfo = null;

    void Awake()
    {
        PrefabInstance();
    }

    /// <summary>
    /// プレハブからインスタンスを生成する
    /// </summary>
    private void PrefabInstance()
    {
        for(int i = 0; i < musicsInfo.Info.Length; i++)
        {
            NodeGameObjectList.Add(Instantiate(prefab, this.transform, false));
            NodeRectTransformList.Add(NodeGameObjectList[i].GetComponent<RectTransform>());
        }

        for(int i = 0; i < musicsInfo.Info.Length; i++)
        {
            MusicNode musicNode = NodeGameObjectList[i].GetComponent<MusicNode>();
            musicNode.MyNodeNum = i;
            musicNode.SetMusicNameText();
            musicNode.SetJacketImage();
        }
    }
}