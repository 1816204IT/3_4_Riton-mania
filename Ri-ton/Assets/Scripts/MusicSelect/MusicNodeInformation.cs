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
    [SerializeField]
    private DifficultyColor difficultyColor;

    private List<GameObject> nodeLists = new List<GameObject>();
    private List<Image> nodeImages = new List<Image>();
    private Image bigNodeBgImage = null;

    void Start()
    {
        bigNodeBgImage = GameObject.FindGameObjectWithTag("BigNode").GetComponent<Image>();
        NullCheck();

        nodeLists = this.GetComponent<MusicNodeInstance>().NodeGameObjectList;
        foreach (GameObject obj in nodeLists)
        {
            nodeImages.Add(obj.GetComponent<Image>());
        }

        difficultyButtonsManager.Initialize();
    }

    public void UpdateInformationByChangeDifficulty()
    {
        Color color = difficultyColor.GetDifficultyColor(SelectedMap.Instance.DifficultyName);

        bigNodeBgImage.color = color;
        foreach (Image image in nodeImages)
        {
            image.color = color;
        }
    }

    private void NullCheck()
    {
        difficultyButtonsManager.IsNull();
        bigNodeBgImage.IsNull();
    }
}