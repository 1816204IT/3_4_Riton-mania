using UnityEngine;
using DG.Tweening;

/// <summary>
/// Easingを用いた拡大縮小を行う
/// </summary>
public class ScaleTween : MonoBehaviour
{
    [SerializeField]
    private float duration = 0.1f;
    private Tween tween = null;
    private RectTransform rectTransform = null;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        NullCheck();
        rectTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
    }

    public void PlayExpandTween()
    {
        tween = rectTransform.DOScale(
            new Vector2(1.0f, 1.0f),
            duration);

        // Easingの設定
        tween.SetEase(Ease.OutQuint);

        tween.Play();
    }

    public void PlayShrinkTween()
    {
        tween = rectTransform.DOScale(
            new Vector2(0.0f, 0.0f),
            duration);

        // Easingの設定
        tween.SetEase(Ease.OutQuint);

        tween.Play();
    }

    private void NullCheck()
    {
        rectTransform.IsNull(nameof(rectTransform));
    }
}
