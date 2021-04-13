using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// チュートリアルクラス
/// </summary>
public class Tutorial : MonoBehaviour
{
    [System.Serializable]
    public struct TutorialData
    {
        public Vector3 unmaskPos;
        public Vector2 unmaskSizeDelta;
        public Vector3 textPos;
        public string text;         // 説明文
        public bool isArrowButtom;  // 矢印画像が下にくるか
    }

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

    private List<TutorialData> words = new List<TutorialData>();
    private AttentionTween textNowTween = null;
    private int index = 0;

    void Start()
    {
        textNowTween = text.transform.GetComponent<AttentionTween>();
        NullCheck();
        TutorialScenarioInit();

        if (UserPreference.instance.isTutorial == false)
        {
            TutorialStart();
        }
    }

    public void TutorialStart()
    {
        index = 0;

        // Canvas有効化
        tutorialCanvas.SetActive(true);

        // 背景の黒を濃くする
        blackUnderLay.color = Color.black;

        // 矢印画像を表示しない
        arrow.SetActive(false);

        // ナビゲーションキャラを表示する
        navigationCharacter.enabled = true;

        // シナリオ開始
        Set();
    }

    private void Set()
    {
        if ( (index == 5) || (index == 10) || (index == 17))
        {
            // ナビゲーションキャラを非表示にする
            navigationCharacter.enabled = false;
            // 背景の黒を薄くする
            blackUnderLay.color = new Color(0, 0, 0, 0.9f);
            // 矢印画像を表示する
            arrow.SetActive(true);
        }
        if ( (index == 8) || (index == 16) )
        {
            // ナビゲーションキャラを表示する
            navigationCharacter.enabled = true;
            // 背景の黒を濃くする
            blackUnderLay.color = Color.black;
            // 矢印画像を非表示にする
            arrow.SetActive(false);
        }
        if (index == 17)
        {
            // ナビゲーションキャラを表示する
            navigationCharacter.enabled = true;
            // 背景の黒を薄くする
            blackUnderLay.color = new Color(0, 0, 0, 0.9f);
        }
        if (index == 19)
        {
            // 背景の黒を濃くする
            blackUnderLay.color = Color.black;
            // 矢印画像を非表示にする
            arrow.SetActive(false);
        }

        var i = words[index];
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
            if (index < words.Count - 1)
            {
                index++;
                Set();
            }
            // チュートリアル終了
            else
            {
                tutorialCanvas.SetActive(false);
                // チュートリアル終了フラグを立てて保存する
                UserPreference.instance.isTutorial = true;
                UserPreference.instance.Save();
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
        data.text = "Ritonmaniaへようこそ！";
        data.isArrowButtom = true; // 矢印画像を表示しないので意味ない
        words.Add(data);

        // 1
        data.unmaskPos = new Vector3(0, 0, 0);
        data.unmaskSizeDelta = new Vector2(0, 0);
        data.textPos = new Vector3(0, 100, 0);
        data.text = "私はナビゲーターのライムです！";
        data.isArrowButtom = true; // 矢印画像を表示しないので意味ない
        words.Add(data);

        // 2
        data.unmaskPos = new Vector3(0, 0, 0);
        data.unmaskSizeDelta = new Vector2(0, 0);
        data.textPos = new Vector3(0, 100, 0);
        data.text = "マスターにゲームの遊び方を説明するよ！";
        data.isArrowButtom = true; // 矢印画像を表示しないので意味ない
        words.Add(data);

        // 3
        data.unmaskPos = new Vector3(0, 0, 0);
        data.unmaskSizeDelta = new Vector2(0, 0);
        data.textPos = new Vector3(0, 100, 0);
        data.text = "よーく聞いて覚えてね！";
        data.isArrowButtom = true; // 矢印画像を表示しないので意味ない
        words.Add(data);

        // 4
        data.unmaskPos = new Vector3(0, 0, 0);
        data.unmaskSizeDelta = new Vector2(0, 0);
        data.textPos = new Vector3(0, 100, 0);
        data.text = "まずは曲の選び方から！";
        data.isArrowButtom = true; // 矢印画像を表示しないので意味ない
        words.Add(data);

        // 5
        data.unmaskPos = new Vector3(0, -265, 0);
        data.unmaskSizeDelta = new Vector2(1280, 150);
        data.textPos = new Vector3(0, -110, 0);
        data.text = "最初に曲を選んで・・・";
        data.isArrowButtom = true;
        words.Add(data);

        // 6
        data.unmaskPos = new Vector3(0, 225, 0);
        data.unmaskSizeDelta = new Vector2(540, 80);
        data.textPos = new Vector3(0, 60, 0);
        data.text = "次に難易度を選んだら・・・";
        data.isArrowButtom = false;
        words.Add(data);

        // 7
        data.unmaskPos = new Vector3(450, -65, 0);
        data.unmaskSizeDelta = new Vector2(273, 250);
        data.textPos = new Vector3(450, 140, 0);
        data.text = "Playボタンでスタート！";
        data.isArrowButtom = true;
        words.Add(data);

        // 8
        data.unmaskPos = new Vector3(0, 0, 0);
        data.unmaskSizeDelta = new Vector2(0, 0);
        data.textPos = new Vector3(0, 100, 0);
        data.text = "とっても簡単だね！";
        data.isArrowButtom = true; // 矢印画像を表示しないので意味ない
        words.Add(data);

        // 9
        data.unmaskPos = new Vector3(0, 0, 0);
        data.unmaskSizeDelta = new Vector2(0, 0);
        data.textPos = new Vector3(0, 100, 0);
        data.text = "続いて、その他の機能の紹介だよ！";
        data.isArrowButtom = true; // 矢印画像を表示しないので意味ない
        words.Add(data);

        // 10
        data.unmaskPos = new Vector3(-463, 0, 0);
        data.unmaskSizeDelta = new Vector2(295, 375);
        data.textPos = new Vector3(-370, 265, 0);
        data.text = "オンラインランキングが表示されるよ！";
        data.isArrowButtom = true;
        words.Add(data);

        // 11
        data.unmaskPos = new Vector3(-470, 223, 0);
        data.unmaskSizeDelta = new Vector2(290, 60);
        data.textPos = new Vector3(-400, 70, 0);
        data.text = "切り替えボタンを押すと・・・";
        data.isArrowButtom = false;
        words.Add(data);

        // 12
        data.unmaskPos = new Vector3(-463, 0, 0);
        data.unmaskSizeDelta = new Vector2(295, 375);
        data.textPos = new Vector3(-370, 265, 0);
        data.text = "自分のハイスコアが表示されるよ！";
        data.isArrowButtom = true;
        words.Add(data);

        // 13
        data.unmaskPos = new Vector3(503, 335, 0);
        data.unmaskSizeDelta = new Vector2(273, 50);
        data.textPos = new Vector3(395, 190, 0);
        data.text = "最後はオプションの説明だよ！";
        data.isArrowButtom = false;
        words.Add(data);

        // 14
        data.unmaskPos = new Vector3(503, 335, 0);
        data.unmaskSizeDelta = new Vector2(273, 50);
        data.textPos = new Vector3(395, 190, 0);
        data.text = "ノーツスピードやタイミング調整等";
        data.isArrowButtom = false;
        words.Add(data);

        // 15
        data.unmaskPos = new Vector3(503, 335, 0);
        data.unmaskSizeDelta = new Vector2(273, 50);
        data.textPos = new Vector3(395, 190, 0);
        data.text = "プレイに関係する設定ができるんだよ！";
        data.isArrowButtom = false;
        words.Add(data);

        // 16
        data.unmaskPos = new Vector3(0, 0, 0);
        data.unmaskSizeDelta = new Vector2(0, 0);
        data.textPos = new Vector3(0, 100, 0);
        data.text = "これで全部だよ！";
        data.isArrowButtom = true; // 矢印画像を表示しないので意味ない
        words.Add(data);

        // 17
        data.unmaskPos = new Vector3(300, 335, 0);
        data.unmaskSizeDelta = new Vector2(100, 50);
        data.textPos = new Vector3(300, 190, 0);
        data.text = "もしも忘れちゃったらココを押してね";
        data.isArrowButtom = false;
        words.Add(data);

        // 18
        data.unmaskPos = new Vector3(300, 335, 0);
        data.unmaskSizeDelta = new Vector2(100, 50);
        data.textPos = new Vector3(300, 190, 0);
        data.text = "ライムが何度でも説明してあげるから！";
        data.isArrowButtom = false;
        words.Add(data);

        // 19
        data.unmaskPos = new Vector3(0, 0, 0);
        data.unmaskSizeDelta = new Vector2(0, 0);
        data.textPos = new Vector3(0, 100, 0);
        data.text = "Ritonmaniaを楽しんでね～♪";
        data.isArrowButtom = true; // 矢印画像を表示しないので意味ない
        words.Add(data);

        // 20
        data.unmaskPos = new Vector3(0, 0, 0);
        data.unmaskSizeDelta = new Vector2(0, 0);
        data.textPos = new Vector3(0, 100, 0);
        data.text = "以上、ナビゲーターのライムでした！";
        data.isArrowButtom = true; // 矢印画像を表示しないので意味ない
        words.Add(data);
    }

    private void NullCheck()
    {
        tutorialCanvas.IsNull();
        unMask.IsNull();
        text.IsNull();
        arrow.IsNull();
        textNowTween.IsNull();
        navigationCharacter.IsNull();
        blackUnderLay.IsNull();
    }
}