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
    [SerializeField]
    private Text difficultyText = null;

    [SerializeField]
    private Font englishFont = null;
    [SerializeField]
    private Font jananeseFont = null;

    private JsonManager jsonManager = null;

    void Awake()
    {
        jsonManager = GameObject.FindGameObjectWithTag("JsonManager").GetComponent<JsonManager>();
        if (jacketImage == null || musicNameText == null || bpmText == null || jsonManager == null
            || authorText == null || mapperText == null || difficultyText == null || englishFont == null
            || jananeseFont == null)
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
        bpmText.text = bpmTextValue.ToString();
        //難易度の更新
        float difficulty = mapData.difficulty / 10.0f;
        difficultyText.text = difficulty.ToString("f1");
        //作者名の更新
        authorText.text = mapInfo.authorName;
        //マッパー名の更新
        mapperText.font = englishFont;
        mapperText.text = mapData.mapperName;
        //ジャケット画像の変更
        jacketImage.sprite = MusicInfoList._instance.GetBgImage(SelectedMap._instance._musicIndex);

        //文字化け対策用チェック
        MapperNameSpecialCheck();
    }

    // マッパー名の日本語が文字化けするので特別にチェック関数を用意
    private void MapperNameSpecialCheck()
    {
        string musicName = SelectedMap._instance._musicName;
        string diffName = SelectedMap._instance._difficultyName;

        // 出汁男のチェック
        if ( (musicName == "くるくる" && diffName == "Hard") 
            || (musicName == "コインランドリー" && diffName == "Hard"))
        {
            //マッパー名の更新
            mapperText.font = jananeseFont;
            mapperText.text = "出汁男";
        }

        // 巻きパンのチェック
        if (musicName == "アンチクワイア")
        {
            if (diffName == "Expert" || diffName == "Hard" || diffName == "Normal")
            {
                //マッパー名の更新
                mapperText.font = jananeseFont;
                mapperText.text = "巻きパン";
            }
        }
    }
}
