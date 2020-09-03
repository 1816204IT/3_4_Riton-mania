using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicNodeInstance : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab = null;

    private List<GameObject> nodeGameObjectList = new List<GameObject>();
    private List<RectTransform> nodeRectTransformList = new List<RectTransform>();

    void Awake()
    {
        PrefabInstance();
    }

    private void PrefabInstance()
    {
        for(int i = 0; i < MusicInfoList._instance._musicNum; i++)
        {
            nodeGameObjectList.Add(Instantiate(prefab, this.transform, false));
            nodeRectTransformList.Add(nodeGameObjectList[i].GetComponent<RectTransform>());
        }

        for(int i = 0; i < MusicInfoList._instance._musicNum; i++)
        {
            MusicNode musicNode = nodeGameObjectList[i].GetComponent<MusicNode>();
            musicNode._myNodeNum = i;
            musicNode.SetMusicNameText();
            musicNode.SetJacketImage();
        }
    }

    public List<GameObject> _nodeGameObjectList
    {
        get { return nodeGameObjectList; }
    }

    public List<RectTransform> _nodeRectTransformList
    { 
        get { return nodeRectTransformList; }
    }
}
