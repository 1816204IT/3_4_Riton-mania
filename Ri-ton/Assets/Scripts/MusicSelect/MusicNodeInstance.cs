using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 曲パネル生成クラス
/// </summary>
public class MusicNodeInstance : MonoBehaviour
{
    public List<GameObject> nodeGameObjectList { get; } = new List<GameObject>();
    public List<RectTransform> nodeRectTransformList { get; } = new List<RectTransform>();

    [SerializeField]
    private GameObject prefab = null;

    void Awake()
    {
        PrefabInstance();
    }

    private void PrefabInstance()
    {
        for(int i = 0; i < MusicInfoList.instance.MusicNum(); i++)
        {
            nodeGameObjectList.Add(Instantiate(prefab, this.transform, false));
            nodeRectTransformList.Add(nodeGameObjectList[i].GetComponent<RectTransform>());
        }

        for(int i = 0; i < MusicInfoList.instance.MusicNum(); i++)
        {
            MusicNode musicNode = nodeGameObjectList[i].GetComponent<MusicNode>();
            musicNode.myNodeNum = i;
            musicNode.SetMusicNameText();
            musicNode.SetJacketImage();
        }
    }
}