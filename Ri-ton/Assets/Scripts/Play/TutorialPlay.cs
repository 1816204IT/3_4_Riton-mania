using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPlay : MonoBehaviour
{
    private List<TutorialData> list = new List<TutorialData>();

    [SerializeField]
    private GameObject tutorialCanvas = null;
    [SerializeField]
    private RectTransform unMask = null;
    [SerializeField]
    private GameObject text = null;
    [SerializeField]
    private GameObject arrow = null;
    [SerializeField]
    private Image navigationCharacter = null;
    [SerializeField]
    private Image blackUnderLay = null;

    private AttentionTween textNowTween = null;

    int index = 0;

    void Start()
    {
        textNowTween = text.transform.GetComponent<AttentionTween>();

        if (tutorialCanvas == null || unMask == null || text == null
            || arrow == null || textNowTween == null || navigationCharacter == null
            || blackUnderLay == null)
        {
            Debug.Log("nullを検知");
        }

        TutorialScenarioInit();
        
        
        index = 0;
        // Canvas有効化
        tutorialCanvas.SetActive(true);
        // 背景の黒を濃くする
        blackUnderLay.color = Color.black;
        // 矢印画像を表示しない
        arrow.SetActive(false);
        // シナリオ開始
        Set();
    }

    private void Set()
    {
        if (index == 5)
        {
            // ナビゲーションキャラを非表示にする
            navigationCharacter.enabled = false;
            // 背景の黒を薄くする
            blackUnderLay.color = new Color(0, 0, 0, 0.9f);
        }
        if (index == 9)
        {
            // ナビゲーションキャラを表示する
            navigationCharacter.enabled = true;
            // 背景の黒を濃くする
            blackUnderLay.color = new Color(0, 0, 0, 1.0f);
        }

        var i = list[index];
        var textRt = text.GetComponent<RectTransform>();

        unMask.localPosition = i.unmaskPos;
        unMask.sizeDelta = i.unmaskSizeDelta;
        textRt.localPosition = i.textPos;
        text.GetComponent<Text>().text = i.text;

        if (i.isArrowButtom)
        {
            ArrowButtomSetting();
            textNowTween.MoveDirDown();
        }
        else
        {
            ArrowTopSetting();
            textNowTween.MoveDirUp();
        }

        // textTweenの設定を変更
        textNowTween.TweenReSetting(textRt.position.y); // ここではlocalPositionではなくPositionを入れる
    }

    private void ArrowButtomSetting()
    {
        RectTransform rt = arrow.GetComponent<RectTransform>();
        rt.rotation = Quaternion.Euler(0, 0, 0);
        rt.localPosition = new Vector3(0, -30, 0);
    }

    private void ArrowTopSetting()
    {
        RectTransform rt = arrow.GetComponent<RectTransform>();
        rt.rotation = Quaternion.Euler(0, 0, 180);
        rt.localPosition = new Vector3(0, 70, 0);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (index < list.Count - 1)
            {
                index++;
                Set();
            }
            // チュートリアル終了
            else
            {
                tutorialCanvas.SetActive(false);
                // チュートリアル終了フラグを立てて保存する
                UserPreference._instance._isTutorial = true;
                UserPreference._instance.Save();
            }
        }
    }

    private void TutorialScenarioInit()
    {
        TutorialData data = new TutorialData();

        // 0
        data.unmaskPos = new Vector3(0, 0, 0);
        data.unmaskSizeDelta = new Vector2(0, 0);
        data.textPos = new Vector3(0, 100, 0);
        data.text = "マスター！！";
        data.isArrowButtom = true; // 矢印画像を表示しないので意味ない
        list.Add(data);

        // 1
        data.unmaskPos = new Vector3(0, 0, 0);
        data.unmaskSizeDelta = new Vector2(0, 0);
        data.textPos = new Vector3(0, 100, 0);
        data.text = "いよいよ初プレイですね！";
        data.isArrowButtom = true; // 矢印画像を表示しないので意味ない
        list.Add(data);

        // 2
        data.unmaskPos = new Vector3(0, 0, 0);
        data.unmaskSizeDelta = new Vector2(0, 0);
        data.textPos = new Vector3(0, 100, 0);
        data.text = "ということでお決まりの～";
        data.isArrowButtom = true; // 矢印画像を表示しないので意味ない
        list.Add(data);

        // 3
        data.unmaskPos = new Vector3(0, 0, 0);
        data.unmaskSizeDelta = new Vector2(0, 0);
        data.textPos = new Vector3(0, 100, 0);
        data.text = "ライムちゃんの説明タイムです！";
        data.isArrowButtom = true; // 矢印画像を表示しないので意味ない
        list.Add(data);

        // 4
        data.unmaskPos = new Vector3(0, 0, 0);
        data.unmaskSizeDelta = new Vector2(0, 0);
        data.textPos = new Vector3(0, 100, 0);
        data.text = "さっそくいきますよ～";
        data.isArrowButtom = true; // 矢印画像を表示しないので意味ない
        list.Add(data);

        // 5
        data.unmaskPos = new Vector3(0, 0, 0);
        data.unmaskSizeDelta = new Vector2(1280, 720);
        data.textPos = new Vector3(0, -110, 0);
        data.text = "ココがプレイエリアだよ";
        data.isArrowButtom = true;
        list.Add(data);

        // 6
        data.unmaskPos = new Vector3(0, 0, 0);
        data.unmaskSizeDelta = new Vector2(1280, 720);
        data.textPos = new Vector3(0, -110, 0);
        data.text = "上からノーツが落ちてくるから";
        data.isArrowButtom = true; // 矢印画像を表示しないので意味ない
        list.Add(data);

        // 7
        data.unmaskPos = new Vector3(0, 0, 0);
        data.unmaskSizeDelta = new Vector2(1280, 720);
        data.textPos = new Vector3(0, -110, 0);
        data.text = "タイミングバーと重なった時にキーを押してね";
        data.isArrowButtom = true; // 矢印画像を表示しないので意味ない
        list.Add(data);

        // 8
        data.unmaskPos = new Vector3(0, 0, 0);
        data.unmaskSizeDelta = new Vector2(1280, 720);
        data.textPos = new Vector3(0, -110, 0);
        data.text = "キーは左から順に「D・F・J・K」だよ";
        data.isArrowButtom = true; // 矢印画像を表示しないので意味ない
        list.Add(data);

        // 9
        data.unmaskPos = new Vector3(0, 0, 0);
        data.unmaskSizeDelta = new Vector2(0, 0);
        data.textPos = new Vector3(0, 100, 0);
        data.text = "それじゃ、頑張ってね！";
        data.isArrowButtom = true; // 矢印画像を表示しないので意味ない
        list.Add(data);
    }
}
