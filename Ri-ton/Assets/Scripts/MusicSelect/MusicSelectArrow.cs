using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

/// <summary>
/// 曲選択の矢印画像のアニメーション
/// マウスポインターが画像の上に来た際に画像を拡大する
/// </summary>
public class MusicSelectArrow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private RectTransform arrowRectTransform = null;
    [SerializeField]
    private ArrowAnimationSynchro animSynchro = null;
    [SerializeField]
    private Image arrowImage = null;

    private const float c_size_fefault = 0.4f;
    private const float c_size_extra_rate = 0.5f;

    private bool isOnPointerEnter = false;
    private Sequence sequence = default;
    private Tween expansionTween = default;
    private Tween shurinkTween = default;
    private Color defaultColor = Color.white;
    private Color mouseOverColor = Color.white;

    void Start()
    {
        NullCheck();
        CreateTween();
        defaultColor = arrowImage.color;
    }

    /// <summary>
    /// Tweenの生成
    /// </summary>
    private void CreateTween()
    {
        sequence = DOTween.Sequence();
        sequence.SetLoops(-1, LoopType.Restart);

        expansionTween.SetEase(Ease.Linear);
        expansionTween = arrowRectTransform.DOScale(new Vector2(c_size_extra_rate, c_size_extra_rate), 0.1f);

        shurinkTween.SetEase(Ease.Linear);
        shurinkTween = arrowRectTransform.DOScale(new Vector2(c_size_fefault, c_size_fefault), 0.1f);

        sequence
            .Append(expansionTween)
            .Append(shurinkTween)
            .AppendInterval(1.0f);

        sequence.Play();
    }

    /// <summary>
    /// 矢印をクリック可能エリアにマウスカーソルが入った際の検知イベント
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        isOnPointerEnter = true;
        arrowImage.color = mouseOverColor;
        animSynchro.PauseAnimation();
        arrowRectTransform.localScale = new Vector2(c_size_extra_rate, c_size_extra_rate);
    }

    /// <summary>
    /// 矢印をクリック可能エリアからマウスカーソルが出た際の検知イベント
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        isOnPointerEnter = false;
        arrowImage.color = defaultColor;
        animSynchro.PlayAnimation();
        arrowRectTransform.localScale = new Vector2(c_size_fefault, c_size_fefault);
    }

    /// <summary>
    /// アニメーション停止
    /// </summary>
    public void PauseAnimation()
    {
        sequence.Pause();
    }

    /// <summary>
    /// アニメーション再生
    /// </summary>
    public void PlayAnimation()
    {
        sequence.Play();
    }

    private void NullCheck()
    {
        arrowRectTransform.IsNull();
        animSynchro.IsNull();
        arrowImage.IsNull();
    }
}
