using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

/// <summary>
/// キャラクター選択画面にて、キャラクターが選択された時の処理を行うクラス
/// </summary>
public class PickUpCharacter : MonoBehaviour
{
    [SerializeField]
    private Color myCharacterColor = Color.white;
    [SerializeField]
    private int characterNum = -1;  // -1は未選択
    [SerializeField]
    private float duration = 0.0f;
    [SerializeField]
    private float appendInterval = 0.0f;
    [SerializeField]
    private GameObject character = null;
    [SerializeField]
    private GameObject canvas = null;
    [SerializeField]
    private PickUpCharacterManager pickUpCharacterManager = null;

    private Sequence sequence;
    private Tween vanishTween = default;
    private Tween appearTween = default;
    private Vector3 defaultScale = default;
    private RectTransform rectTransform = null;

    void Start()
    {
        Button button = GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();
        NullCheck();
        defaultScale = rectTransform.localScale;
        button.onClick.AddListener(OnPickUp);
    }

    /// <summary>
    /// キャラクターが両サイドから消滅するTweenの生成
    /// </summary>
    private void CreateVanishTween()
    {
        vanishTween = rectTransform.DOScale(new Vector3(0.0f, defaultScale.y, defaultScale.z), duration);
        vanishTween.SetEase(Ease.OutQuint);
        sequence = DOTween.Sequence();
        sequence
            .Append(vanishTween)
            .AppendInterval(appendInterval)
            .AppendCallback(() => pickUpCharacterManager.OnVanishComplete());
    }

    /// <summary>
    /// キャラクターが両サイドから出現するTweenの生成
    /// </summary>
    private void CreateAppearTween()
    {
        appearTween = rectTransform.DOScale(defaultScale, duration);
        appearTween.SetEase(Ease.OutQuint);
        sequence = DOTween.Sequence();
        sequence
            .Append(appearTween)
            .AppendInterval(appendInterval)
            .AppendCallback(() => pickUpCharacterManager.OnAppearComplete());
    }

    /// <summary>
    /// キャラクターが両サイドから消滅するTweenの実行
    /// </summary>
    public void PlayVanishTween()
    {
        CreateVanishTween();
        this.gameObject.SetActive(false);
        sequence.Play();
    }

    /// <summary>
    /// キャラクターが両サイドから出現するTweenの実行
    /// </summary>
    public void PlayAppearTween()
    {
        CreateAppearTween();
        this.gameObject.SetActive(true);
        sequence.Play();
    }

    /// <summary>
    /// キャラクターが選択された時に呼ばれるコールバック関数
    /// </summary>
    private void OnPickUp()
    {
        GameObject obj = CopyMyCharacterImage();
        pickUpCharacterManager.OnPickUp(obj);
        pickUpCharacterManager.PickingCharacterNum = characterNum;
    }

    /// <summary>
    /// 選択されたキャラクター画像(image)のクローンを作成する
    /// </summary>
    /// <returns>作成したGameObject</returns>
    public GameObject CopyMyCharacterImage()
    {
        GameObject obj = Instantiate(character);
        obj.transform.parent = canvas.transform;
        obj.transform.position = character.transform.position;
        obj.GetComponent<Image>().raycastTarget = false;    // レイキャストターゲットOFF
        return obj;
    }

    private void NullCheck()
    {
        character.IsNull();
        canvas.IsNull();
        pickUpCharacterManager.IsNull();
    }
}
