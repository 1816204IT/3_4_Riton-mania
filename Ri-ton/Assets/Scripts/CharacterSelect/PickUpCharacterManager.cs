using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// キャラクター選択画面の管理クラス
/// </summary>
public class PickUpCharacterManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] hideObjects = null;

    [SerializeField]
    private PickUpCharacter[] characters = null;
    [SerializeField]
    private GameObject moveEndPos = null;

    private GameObject moveCharacter = null;
    private int vanishCompNum = 0;

    [SerializeField]
    private ScaleTween[] scaleTweens = null;

    [SerializeField]
    private HeaderInfo headerInfo = null;

    public int pickingCharacterNum { private get; set; } = -1; // 選択中のキャラクター画像番号

    [SerializeField]
    private MoveTween whiteBackImage = null;
    [SerializeField]
    private MoveTween colorBackImage = null;
    [SerializeField]
    private TextMeshProUGUI characterNameText = null;
    [SerializeField]
    private Material characterNameMat = null;

    [SerializeField]
    private CharacterProfileData characterProfileData = null;
    [SerializeField]
    private GameObject profile = null;

    [SerializeField]
    private RectTransform nowArrow = null;
    private float nowArrowDistanceX = 220;  // 配置しているキャラクター同士の距離

    [SerializeField]
    private GameObject pleasePickCharacter = null;


    // キャラクター選択画面のホーム(初期画面)にいるか？　ホーム画面とそれ以外でEscapeキーを押した時の挙動が変わる
    enum NOW_STATE
    {
        HOME,               // キャラクター選択シーンのホーム画面
        TWEENING,           // tweenアニメーションの最中
        CHARACTER_DECIDE,   // 「このキャラクターにしますか？」　の画面
    }
    private NOW_STATE nowState = NOW_STATE.HOME;

    void Start()
    {
        NullCheck();
        profile.SetActive(false);
        // キャラクターの番号を取得
        pickingCharacterNum = UserPreference.instance._characterNum;
        // nowの矢印のX座標移動
        MoveNowArrow();
        // 「キャラクターを選んで下さい」のキャンバスを非表示に
        pleasePickCharacter.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickBackButton();
        }
    }

    // キャラクターを選んだとき
    public void OnPickUp(GameObject createObj, Color selectedCharacterColor)
    {
        nowState = NOW_STATE.TWEENING;
        moveCharacter = createObj;
        vanishCompNum = 0;
        foreach (PickUpCharacter c in characters)
        {
            c.PlayVanishTween();
        }

        foreach (GameObject obj in hideObjects)
        {
            obj.SetActive(false);
        }
    }

    public void OnPickDown()
    {
        foreach (PickUpCharacter c in characters)
        {
            c.PlayAppearTween();
        }

        foreach (GameObject obj in hideObjects)
        {
            obj.SetActive(true);
        }
    }

    // キャラクターの消滅完了
    public void OnVanishComplete()
    {
        vanishCompNum++;
        if (vanishCompNum >= characters.Length)
        {
            // キャラクターを移動させる為、moveCharacterにコンポーネントを追加
            moveCharacter.AddComponent<MoveTween>();
            MoveTween pickUpCharacter = moveCharacter.GetComponent<MoveTween>();
            pickUpCharacter.MoveCharacter(moveEndPos, this.GetComponent<PickUpCharacterManager>());
        }
    }

    // キャラクターの出現完了
    public void OnAppearComplete()
    {
        nowState = NOW_STATE.HOME;
    }

    // キャラクターの中央移動完了
    public void OnCharacterMoveComplete()
    {
        // キャラクターデータの取得
        Ritonmania.CharacterData data = characterProfileData.GetCharacterData(pickingCharacterNum);
        Color color = CharacterInfoList.instance.GetColor(pickingCharacterNum);

        // 背景の名前の色とtextを指定
        characterNameText.color = color;
        //characterNameText.text = data.name;

        characterNameText.text = data.name;
        characterNameMat.SetColor("_FaceColor", color);

        // 背景の色を指定
        colorBackImage.GetComponent<Image>().color = color;
        // 背景のTween開始
        whiteBackImage.MoveBG(this.GetComponent<PickUpCharacterManager>());
        colorBackImage.MoveBG(this.GetComponent<PickUpCharacterManager>());

        // キャラクタープロフィールテキスト更新
        profile.GetComponent<ProfileSetter>().UpdateProfile(data);
    }

    // キャラクターの定位置移動完了
    public void OnCharacterMoveRevertComplete()
    {
        // moveCharacterを削除する(クローンなので削除しないと増えていく)
        Destroy(moveCharacter);

        OnPickDown();
    }

    // 背景の出現完了
    public void OnAppearBGComplete()
    {
        // SELECTボタンTween開始
        foreach (ScaleTween tween in scaleTweens)
        {
            tween.PlayExpandTween();
        }

        // プロフィール表示
        profile.SetActive(true);

        nowState = NOW_STATE.CHARACTER_DECIDE;
    }

    // 背景の消滅完了
    public void OnVanishBGComplete()
    {
        // キャラクターの定位置への移動開始
        MoveTween pickUpCharacter = moveCharacter.GetComponent<MoveTween>();
        pickUpCharacter.MoveRevertCharacter(this);
    }

    // Backボタンが押された時の処理
    public void OnClickBackButton()
    {
        // キャラクター選択シーンのホーム画面にいる場合タイトルシーンへ遷移
        if (nowState == NOW_STATE.HOME)
        {
            // キャラクター未選択なら催促分を表示
            if ((pickingCharacterNum < 0) || (pickingCharacterNum > 4))
            {
                pleasePickCharacter.SetActive(true);
                // 催促分を指定秒数後に消す
                Invoke("UnEnablePleasePickCharacterObj", 1.0f);
            }
            // キャラクター選択済みならタイトルへ戻る
            else
            {
                UserPreference.instance.AsyncCharacterIcon();   // サーバー同期
                SceneManager.LoadScene("Title");
            }
        }
        // 「このキャラクターにしますか？」の画面にいる場合はホームシーンへ遷移
        else if (nowState == NOW_STATE.CHARACTER_DECIDE)
        {
            // 背景のTween開始
            whiteBackImage.MoveRevertBG(this.GetComponent<PickUpCharacterManager>());
            colorBackImage.MoveRevertBG(this.GetComponent<PickUpCharacterManager>());

            // SELECTボタン縮小Tween開始
            foreach (ScaleTween tween in scaleTweens)
            {
                tween.PlayShrinkTween();
            }

            // プロフィール非表示
            profile.SetActive(false);
        }
        // tweenアニメーションの最中なら何もしない
        else { }
    }

    public void ChangeCharacter()
    {
        // キャラクターの番号を変更
        UserPreference.instance._characterNum = pickingCharacterNum;
        // ヘッダーのプレイヤー名の文字色を変更
        headerInfo.SetPlayerNameColor();
        // nowの矢印のX座標移動
        MoveNowArrow();
        // 戻るボタンが押された時の処理をする
        OnClickBackButton();
    }

    // nowの矢印のX座標移動
    private void MoveNowArrow()
    {
        if ((pickingCharacterNum < 0)  || (pickingCharacterNum > 4))
        {
            return;
        }

        Vector3 p = nowArrow.transform.localPosition;
        float posX = nowArrowDistanceX * (pickingCharacterNum - 2); // キャラクター番号2のキャラクターが中央に配置されているため -2 する
        nowArrow.transform.localPosition = new Vector3(posX, p.y, p.z);
    }

    private void UnEnablePleasePickCharacterObj()
    {
        pleasePickCharacter.SetActive(false);
    }

    private void NullCheck()
    {
        moveEndPos.IsNull(nameof(moveEndPos));
        headerInfo.IsNull(nameof(headerInfo));
        whiteBackImage.IsNull(nameof(whiteBackImage));
        colorBackImage.IsNull(nameof(colorBackImage));
        characterProfileData.IsNull(nameof(characterProfileData));
        characterNameText.IsNull(nameof(characterNameText));
        profile.IsNull(nameof(profile));
        nowArrow.IsNull(nameof(nowArrow));
        pleasePickCharacter.IsNull(nameof(pleasePickCharacter));

        if (characters.Length == 0)
        {
            Debug.LogError("characters is Null");
        }
    }
}
