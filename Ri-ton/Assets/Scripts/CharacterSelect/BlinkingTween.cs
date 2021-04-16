using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 透明度を用いた点滅表現を行う
/// Text/TextMeshPro/Imageに対応
/// </summary>
public class BlinkingTween : MonoBehaviour
{
    [SerializeField]
    private float duration = 0.0f;          // 不透明から透明、透明から不透明になるまでの時間
    [SerializeField]
    private float appendInterval = 0.0f;    // 次の点滅までのインターバル時間

    private MaskableGraphic uiComponent = null;

    void Start()
    {
        uiComponent = GetComponent<MaskableGraphic>();
        NullCheck();
        CreateTween();
    }

    /// <summary>
    /// Tweenの生成
    /// </summary>
    private void CreateTween()
    {
        Sequence sequence = DOTween.Sequence();
        Tween tween = default;

        // Easingの設定
        tween.SetEase(Ease.OutQuint);
        // 透明にするTween
        tween = DOTween.ToAlpha(() => uiComponent.color, color => uiComponent.color = color, 0.0f, duration);
        // sequenceに追加
        sequence.Append(tween);

        // Easingの設定
        tween.SetEase(Ease.InQuint);
        // 透明から元に戻すTween
        tween = DOTween.ToAlpha(() => uiComponent.color, color => uiComponent.color = color, 1.0f, duration);
        // sequenceに追加
        sequence.Append(tween);

        // 待ち時間を追加
        sequence.PrependInterval(appendInterval);
        // ループの設定
        sequence.SetLoops(-1, LoopType.Restart);

        sequence.Play();
    }

    private void NullCheck()
    {
        uiComponent.IsNull();
    }
}
