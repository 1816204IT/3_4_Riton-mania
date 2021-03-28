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

    [SerializeField]
    private ColorOfDifficulty colorOfDifficulty;

    void Start()
    {
        bigNodeBgImage = GameObject.FindGameObjectWithTag("BigNode").GetComponent<Image>();

        if (difficultyButtonsManager == null || bigNodeBgImage == null)
        {
            Debug.Log("nullを検知");
        }

        nodeList = this.GetComponent<MusicNodeInstance>().nodeGameObjectList;
        foreach (GameObject obj in nodeList)
        {
            nodeImages.Add(obj.GetComponent<Image>());
        }

        difficultyButtonsManager.Initialize();
    }

    public void UpdateInformationByChangeDifficulty()
    {
        Color color = colorOfDifficulty.GetColorOfDifficulty(SelectedMap.instance._difficultyName);

        bigNodeBgImage.color = color;
        foreach (Image image in nodeImages)
        {
            image.color = color;
        }
    }
}