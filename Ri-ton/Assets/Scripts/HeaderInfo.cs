using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeaderInfo : MonoBehaviour
{
    [SerializeField]
    private Text playerNameText = null;

    void Start()
    {
        if (playerNameText == null)
        {
            Debug.Log("nullを検知");
        }
    }

    private void Update()
    {
        string playerName = FindObjectOfType<UserAuth>()._playerName;
        if (playerName == null)
        {
            playerName = "NOT LOGIN";
        }
        playerNameText.text = playerName;
    }
}
