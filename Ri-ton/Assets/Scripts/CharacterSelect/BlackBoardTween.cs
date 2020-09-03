using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlackBoardTween : MonoBehaviour
{
    [SerializeField]
    private float duration = 0.1f;
    private Tween expandTween = null;
    private Tween shurinkTween = null;

    private RectTransform rectTransform = null;
    [SerializeField]
    private float expand_width = 2000.0f;
    [SerializeField]
    private float expand_height = 1000.0f;
    private float defaultWidth;
    private float defaultHeight;


    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        defaultWidth = rectTransform.sizeDelta.x;
        defaultHeight = rectTransform.sizeDelta.y;
    }

    public void PlayExpandTween()
    {
        // 拡大Tween
        expandTween = rectTransform.DOSizeDelta(
            new Vector2(expand_width, expand_height),
            duration);

        // Easingの設定
        expandTween.SetEase(Ease.OutQuint);

        expandTween.Play();
    }

    public void PlayShurinkTween()
    {
        // 縮小Tween
        shurinkTween = rectTransform.DOSizeDelta(
            new Vector2(defaultWidth, defaultHeight),
            duration);

        // Easingの設定
        shurinkTween.SetEase(Ease.OutQuint);

        shurinkTween.Play();
    }
}
