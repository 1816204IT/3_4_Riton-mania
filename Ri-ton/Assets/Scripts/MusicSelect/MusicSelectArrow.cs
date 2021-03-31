using System.Collections;
using System.Collections.Generic;
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
    private bool isOnPointerEnter = false;

    [SerializeField]
    private RectTransform arrowRectTransform = null;
    [SerializeField]
    private ArrowAnimationSynchro animSynchro = null;

    private Sequence sequence = default;
    private Tween expansionTween = default;
    private Tween shurinkTween = default;

    [SerializeField]
    private Image arrowImage = null;
    private Color defaultColor = Color.white;
    private Color mouseOverColor = Color.white;

    private const float size_default = 0.4f;
    private const float size_extra_rate = 0.5f;

    void Start()
    {
        NullCheck();
        TweenInit();
        defaultColor = arrowImage.color;
    }

    private void TweenInit()
    {
        sequence = DOTween.Sequence();
        sequence.SetLoops(-1, LoopType.Restart);

        expansionTween.SetEase(Ease.Linear);
        expansionTween = arrowRectTransform.DOScale(new Vector2(size_extra_rate, size_extra_rate), 0.1f);

        shurinkTween.SetEase(Ease.Linear);
        shurinkTween = arrowRectTransform.DOScale(new Vector2(size_default, size_default), 0.1f);

        sequence
            .Append(expansionTween)
            .Append(shurinkTween)
            .AppendInterval(1.0f);

        sequence.Play();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isOnPointerEnter = true;
        arrowImage.color = mouseOverColor;
        animSynchro.PauseAnimation();
        arrowRectTransform.localScale = new Vector2(size_extra_rate, size_extra_rate);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOnPointerEnter = false;
        arrowImage.color = defaultColor;
        animSynchro.PlayAnimation();
        arrowRectTransform.localScale = new Vector2(size_default, size_default);
    }

    public void PauseAnimation()
    {
        sequence.Pause();
    }

    public void PlayAnimation()
    {
        sequence.Play();
    }

    private void NullCheck()
    {
        arrowRectTransform.IsNull(nameof(arrowRectTransform));
        animSynchro.IsNull(nameof(animSynchro));
        arrowImage.IsNull(nameof(arrowImage));
    }
}
