using UnityEngine;
using DG.Tweening;

/// <summary>
/// Easingを用いた拡大縮小を行う
/// </summary>
public class ScaleTween : MonoBehaviour
{
    [SerializeField]
    private float duration = 0.0f;
    [SerializeField]
    private float min = 0.0f;   // 最小のときの拡大率
    [SerializeField]
    private float max = 1.0f;   // 最大のときの拡大率

    private Tween expandTween = null;
    private Tween shrinkTween = null;
    private RectTransform rectTransform = null;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        NullCheck();
        rectTransform.localScale = Vector3.zero;

        CreateExpandTween();
        CreateShrinkTween();
    }

    /// <summary>
    /// 拡大Tweenの生成
    /// </summary>
    private void CreateExpandTween()
    {
        expandTween = rectTransform.DOScale(new Vector2(max, max), duration);
        expandTween.SetEase(Ease.OutQuint);
    }

    /// <summary>
    /// 縮小Tweenの生成
    /// </summary>
    private void CreateShrinkTween()
    {
        shrinkTween = rectTransform.DOScale(new Vector2(min, min), duration);
        shrinkTween.SetEase(Ease.OutQuint);
    }

    /// <summary>
    /// 拡大Tweenの再生
    /// </summary>
    public void PlayExpandTween()
    {
        expandTween.Play();
    }

    /// <summary>
    /// 縮小Tweenの再生
    /// </summary>
    public void PlayShrinkTween()
    {
        expandTween.Play();
    }

    private void NullCheck()
    {
        rectTransform.IsNull();
    }
}
