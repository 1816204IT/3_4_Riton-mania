using NoteEditor.DTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BigNodeInformation : MonoBehaviour
{
    [SerializeField]
    private Image jacketImage = null;
    [SerializeField]
    private Text musicNameText = null;
    [SerializeField]
    private Text bpmText = null;
    [SerializeField]
    private Text authorText = null;
    [SerializeField]
    private Text mapperText = null;

    private JsonManager jsonManager = null;

    void Awake()
    {
        jsonManager = GameObject.FindGameObjectWithTag("JsonManager").GetComponent<JsonManager>();
        if (jacketImage == null || musicNameText == null || bpmText == null || jsonManager == null
            || authorText == null || mapperText == null)
        {
            Debug.Log("nullを検知");
        }
        InformationUpdate();
    }

    public void InformationUpdate()
    {
        MapInfo mapInfo = jsonManager.LoadMapInfo(SelectedMap._instance._musicName);
        MusicDTO.MapData mapData = jsonManager.LoadMapData(SelectedMap._instance._musicName, SelectedMap._instance._difficultyName);

        //曲名の更新
        musicNameText.text = SelectedMap._instance._musicName;
        //BPMの更新
        float bpmTextValue = mapInfo.bpm / 100.0f;
        bpmText.text = "BPM:" + bpmTextValue.ToString();
        //作者名の更新
        authorText.text = "Author : " + mapInfo.authorName;
        //マッパー名の更新
        mapperText.text = "Mapper : " + mapData.mapperName;
        //ジャケット画像の変更
        jacketImage.sprite = MusicInfoList._instance.GetBgImage(SelectedMap._instance._musicIndex);
    }
}
