using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 曲パネルの情報を設定するクラス
/// </summary>
public class MusicNodeInformation : MonoBehaviour
{
    [SerializeField]
    private DifficultyButtonsManager difficultyButtonsManager = null;

    private List<GameObject> nodeList = new List<GameObject>();
    private List<Image> nodeImages = new List<Image>();
    private Image bigNodeBgImage = null;

    void Start()
    {
        bigNodeBgImage = GameObject.FindGameObjectWithTag("BigNode").GetComponent<Image>();

        if (difficultyButtonsManager == null || bigNodeBgImage == null)
        {
            Debug.Log("nullを検知");
        }

        nodeList = this.GetComponent<MusicNodeInstance>()._nodeGameObjectList;
        foreach (GameObject obj in nodeList)
        {
            nodeImages.Add(obj.GetComponent<Image>());
        }

        difficultyButtonsManager.Initialize();
    }

    public void UpdateInformationByChangeDifficulty()
    {
        bigNodeBgImage.color = ColorOfDifficulty._instance.GetColorOfDifficulty();
        foreach (Image image in nodeImages)
        {
            image.color = ColorOfDifficulty._instance.GetColorOfDifficulty();
        }
        foreach (GameObject obj in nodeList)
        {
            obj.transform.Find("inLine").gameObject.GetComponent<Image>().color = ColorOfDifficulty._instance.GetColorOfDifficulty();
        }
    }
}
