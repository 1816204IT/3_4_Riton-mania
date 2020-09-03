using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayButtonTween : MonoBehaviour
{
    private Sequence sequence = default;
    private Tween expansionTween = default;
    private Tween shurinkTween = default;

    private RectTransform rectTransform = null;

    private const float size_default = 0.4f;
    private const float size_extra_rate = 0.5f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.Log("nullを検知");
        }

        sequence = DOTween.Sequence();
        sequence.SetLoops(-1, LoopType.Restart);

        expansionTween.SetEase(Ease.Linear);
        expansionTween = rectTransform.DOScale(new Vector2(size_extra_rate, size_extra_rate), 0.1f);

        shurinkTween.SetEase(Ease.Linear);
        shurinkTween = rectTransform.DOScale(new Vector2(size_default, size_default), 0.1f);

        sequence
            .Append(expansionTween)
            //.AppendInterval(0.1f)
            .Append(shurinkTween)
            .AppendInterval(1.0f);


        sequence.Play();
    }

    void Update()
    {
        
    }
}
