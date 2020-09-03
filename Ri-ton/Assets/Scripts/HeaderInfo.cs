using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeaderInfo : MonoBehaviour
{
    [SerializeField]
    private Image iconImage = null;
    [SerializeField]
    private Text playerNameText = null;

    void Start()
    {
        if (iconImage == null || playerNameText == null)
        {
            Debug.Log("nullを検知");
        }
    }

    private void Update()
    {
        int num = UserPreference._instance._characterNum;
        iconImage.sprite = CharacterImageList._instance.GetIconSprite(num);

        string playerName = FindObjectOfType<UserAuth>()._playerName;
        if (playerName == null)
        {
            playerName = "NOT LOGIN";
        }
        playerNameText.text = playerName;
    }
}
