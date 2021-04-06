using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

/// <summary>
/// キャラクター選択画面にてキャラクターが選択された時にキャラクターを両サイドから消す
/// </summary>
public class PickUpCharacter : MonoBehaviour
{
    [SerializeField]
    private Color myCharacterColor = Color.white;
    [SerializeField]
    private int characterNum = -1;
    [SerializeField]
    private float duration = 0.1f;
    [SerializeField]
    private float appendInterval = 0.1f;
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
        // キャラクターが選択された時のコールバックを設定
        button.onClick.AddListener(OnPickUp);
    }

    private void CreateVanishTween()
    {
        // 両サイドから消えるTweenの作成
        vanishTween = rectTransform.DOScale(
            new Vector3(0.0f, defaultScale.y, defaultScale.z),
            duration);

        // Easingの設定
        vanishTween.SetEase(Ease.OutQuint);

        sequence = DOTween.Sequence();

        sequence
            .Append(vanishTween)
            .AppendInterval(appendInterval)
            .AppendCallback(() => pickUpCharacterManager.OnVanishComplete());
    }

    private void CreateAppearTween()
    {
        // 両サイドから現れるTweenの作成
        appearTween = rectTransform.DOScale(
            defaultScale,
            duration);

        // Easingの設定
        appearTween.SetEase(Ease.OutQuint);

        sequence = DOTween.Sequence();

        sequence
            .Append(appearTween)
            .AppendInterval(appendInterval)
            .AppendCallback(() => pickUpCharacterManager.OnAppearComplete());
    }

    // 両サイドから消えていく
    public void PlayVanishTween()
    {
        CreateVanishTween();
        this.gameObject.SetActive(false);
        sequence.Play();
    }

    // 両サイドから現れる
    public void PlayAppearTween()
    {
        CreateAppearTween();
        this.gameObject.SetActive(true);
        sequence.Play();
    }

    // キャラクターが選択された時に呼ばれるコールバック関数
    private void OnPickUp()
    {
        GameObject obj = CopyMyCharacterImage();
        pickUpCharacterManager.OnPickUp(obj, myCharacterColor);
        pickUpCharacterManager.pickingCharacterNum = characterNum;
    }

    // 自身のキャラクター画像(image)のクローンを作成する
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
        character.IsNull(nameof(character));
        canvas.IsNull(nameof(canvas));
        pickUpCharacterManager.IsNull(nameof(pickUpCharacterManager));
    }
}
