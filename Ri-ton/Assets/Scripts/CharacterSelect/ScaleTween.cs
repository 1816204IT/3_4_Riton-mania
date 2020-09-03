using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleTween : MonoBehaviour
{
    private Tween tween = null;
    private RectTransform rectTransform = null;
    [SerializeField]
    private float duration = 0.1f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.Log("nullを検知");
        }

        rectTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
    }

    public void PlayExpandTween()
    {
        tween = rectTransform.DOScale(
            new Vector2(1.0f, 1.0f),
            duration);

        // Easingの設定
        //tween.SetEase(Ease.OutBack);
        tween.SetEase(Ease.OutQuint);

        tween.Play();
    }

    public void PlayShrinkTween()
    {
        tween = rectTransform.DOScale(
            new Vector2(0.0f, 0.0f),
            duration);

        // Easingの設定
        //tween.SetEase(Ease.OutBack);
        tween.SetEase(Ease.OutQuint);

        tween.Play();
    }
}
