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
    private DifficultyColor difficultyColor;

    void Start()
    {
        bigNodeBgImage = GameObject.FindGameObjectWithTag("BigNode").GetComponent<Image>();
        NullCheck();

        nodeList = this.GetComponent<MusicNodeInstance>().nodeGameObjectList;
        foreach (GameObject obj in nodeList)
        {
            nodeImages.Add(obj.GetComponent<Image>());
        }

        difficultyButtonsManager.Initialize();
    }

    public void UpdateInformationByChangeDifficulty()
    {
        Color color = difficultyColor.GetDifficultyColor(SelectedMap.instance.difficultyName);

        bigNodeBgImage.color = color;
        foreach (Image image in nodeImages)
        {
            image.color = color;
        }
    }

    private void NullCheck()
    {
        difficultyButtonsManager.IsNull(nameof(difficultyButtonsManager));
        bigNodeBgImage.IsNull(nameof(bigNodeBgImage));
    }
}