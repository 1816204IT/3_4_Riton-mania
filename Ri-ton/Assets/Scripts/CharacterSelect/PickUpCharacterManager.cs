using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// キャラクター選択画面の管理クラス
/// </summary>
public class PickUpCharacterManager : MonoBehaviour
{
    public int PickingCharacterNum { private get; set; } = -1; // 選択中のキャラクター番号　-1は未選択

    [SerializeField]
    private GameObject[] hideObjects = null;
    [SerializeField]
    private PickUpCharacter[] characters = null;
    [SerializeField]
    private GameObject moveEndPos = null;
    [SerializeField]
    private ScaleTween[] scaleTweens = null;
    [SerializeField]
    private HeaderInfo headerInfo = null;
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
    [SerializeField]
    private GameObject pleasePickCharacter = null;

    private const float c_character_distance_x = 220;  // 配置しているキャラクター同士の距離

    private GameObject moveCharacter = null;
    private int vanishCompNum = 0;
    private NowScreenState nowState = NowScreenState.HOME;

    /// <summary>
    /// キャラクター選択シーンの中で、現在どの画面にいるか。
    /// 現在の場面によってEscapeキーの処理を変更しています
    /// </summary>
    private enum NowScreenState
    {
        HOME,               // キャラクター選択シーンのホーム画面
        TWEENING,           // tweenアニメーションの最中
        CHARACTER_DECIDE,   // キャラクター決定画面
    }

    void Start()
    {
        NullCheck();
        profile.SetActive(false);
        PickingCharacterNum = UserPreference.Instance.GetCharacterNumber();
        MoveNowArrow();
        pleasePickCharacter.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickBackButton();
        }
    }

    /// <summary>
    /// キャラクターを選択した際の処理
    /// </summary>
    /// <param name="characterImageObj">キャラクター画像オブジェクト</param>
    public void OnPickUp(GameObject characterImageObj)
    {
        nowState = NowScreenState.TWEENING;
        moveCharacter = characterImageObj;
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

    /// <summary>
    /// キャラクター決定画面からキャラクター選択ホーム画面へ戻る際の処理
    /// </summary>
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

    /// <summary>
    /// キャラクター選択後、キャラクター選択ボックスが消滅した際の処理
    /// </summary>
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

    /// <summary>
    /// キャラクター決定画面からキャラクター選択ホーム画面へ戻った際に、キャラクター選択ボックスが出現した際の処理
    /// </summary>
    public void OnAppearComplete()
    {
        nowState = NowScreenState.HOME;
    }

    /// <summary>
    /// キャラクターの中央移動完了後の処理
    /// </summary>
    public void OnCharacterMoveComplete()
    {
        // キャラクターデータの取得
        Ritonmania.CharacterData data = characterProfileData.GetCharacterData(PickingCharacterNum);
        Color color = CharacterInfoList.instance.GetColor(PickingCharacterNum);

        // 名前の色とtextを指定
        characterNameText.color = color;
        characterNameText.text = data.name;
        characterNameMat.SetColor("_FaceColor", color);

        // 背景の色を指定
        colorBackImage.GetComponent<Image>().color = color;
        // 背景のTween開始
        whiteBackImage.SlideBG(this.GetComponent<PickUpCharacterManager>());
        colorBackImage.SlideBG(this.GetComponent<PickUpCharacterManager>());

        // キャラクタープロフィールテキスト更新
        profile.GetComponent<ProfileSetter>().UpdateProfile(data);
    }

    /// <summary>
    /// キャラクターの定位置移動完了後の処理
    /// </summary>
    public void OnCharacterMoveRevertComplete()
    {
        // moveCharacterを削除する(クローンなので削除しないと増えていく)
        Destroy(moveCharacter);

        OnPickDown();
    }

    /// <summary>
    /// 背景の出現完了後の処理
    /// </summary>
    public void OnAppearBGComplete()
    {
        // SELECTボタンTween開始
        foreach (ScaleTween tween in scaleTweens)
        {
            tween.PlayExpandTween();
        }

        profile.SetActive(true);
        nowState = NowScreenState.CHARACTER_DECIDE;
    }

    /// <summary>
    /// 背景の消滅完了後の処理
    /// </summary>
    public void OnVanishBGComplete()
    {
        // キャラクターの定位置への移動開始
        MoveTween pickUpCharacter = moveCharacter.GetComponent<MoveTween>();
        pickUpCharacter.MoveRevertCharacter(this);
    }

    /// <summary>
    /// Backボタンが押された時の処理
    /// </summary>
    public void OnClickBackButton()
    {
        // キャラクター選択シーンのホーム画面にいる場合タイトルシーンへ遷移
        if (nowState == NowScreenState.HOME)
        {
            int characterNum = UserPreference.Instance.GetCharacterNumber();
            // キャラクター未選択なら催促分を表示
            if ((PickingCharacterNum < 0) || (PickingCharacterNum > 4))
            {
                pleasePickCharacter.SetActive(true);
                // 催促分を指定秒数後に消す
                Invoke("UnEnablePleasePickCharacterObj", 1.0f);
            }
            // キャラクター選択済みならタイトルへ戻る
            else
            {
                UserPreference.Instance.AsyncCharacterIcon();   // サーバー同期
                SceneManager.LoadScene("Title");
            }
        }
        // 「このキャラクターにしますか？」の画面にいる場合はホームシーンへ遷移
        else if (nowState == NowScreenState.CHARACTER_DECIDE)
        {
            // 背景のTween開始
            whiteBackImage.RevertSlideBG(this.GetComponent<PickUpCharacterManager>());
            colorBackImage.RevertSlideBG(this.GetComponent<PickUpCharacterManager>());

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

    /// <summary>
    /// キャラクターを変更した際の処理
    /// </summary>
    public void ChangeCharacter()
    {
        // キャラクターの番号を変更
        UserPreference.Instance.SetCharacterNumber(PickingCharacterNum);
        // ヘッダーのプレイヤー名の文字色を変更
        headerInfo.SetPlayerNameColor();
        // nowの矢印のX座標移動
        MoveNowArrow();
        // 戻るボタンが押された時の処理をする
        OnClickBackButton();
    }

    /// <summary>
    /// nowの矢印を移動させる
    /// </summary>
    private void MoveNowArrow()
    {
        if ((PickingCharacterNum < 0)  || (PickingCharacterNum > 4))
        {
            return;
        }

        Vector3 p = nowArrow.transform.localPosition;
        float posX = c_character_distance_x * (PickingCharacterNum - 2); // キャラクター番号2のキャラクターが中央に配置されているため -2 する
        nowArrow.transform.localPosition = new Vector3(posX, p.y, p.z);
    }

    private void NullCheck()
    {
        moveEndPos.IsNull();
        headerInfo.IsNull();
        whiteBackImage.IsNull();
        colorBackImage.IsNull();
        characterProfileData.IsNull();
        characterNameText.IsNull();
        profile.IsNull();
        nowArrow.IsNull();
        pleasePickCharacter.IsNull();

        if (characters.Length == 0)
        {
            Debug.LogError("characters is Null");
        }
    }
}
