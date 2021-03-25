using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 透明度を用いた点滅表現を行う
/// Text/TextMeshPro/Imageに対応
/// </summary>
public class BlinkingTween : MonoBehaviour
{
    // 点滅描画に対応しているUI種別
    private enum UIType
    { 
        Text,
        TextMeshPro,
        Image,
    }

    // 点滅描画対象のUI種別
    [SerializeField]
    private UIType uiType = UIType.Text;

    // 不透明から透明、透明から不透明になるまでの時間
    [SerializeField]
    private float duration = 0.5f;

    // 次の点滅までのインターバル時間
    [SerializeField]
    private float appendInterval = 1.0f;

    private MaskableGraphic ui = null;

    void Start()
    {
        NullCheck();
        CreateTween();
    }

    // 点滅表示用Tweenの生成
    private void CreateTween()
    {
        Sequence sequence = DOTween.Sequence();
        Tween tween = default;

        // Easingの設定
        tween.SetEase(Ease.OutQuint);
        // 透明にするTween
        tween = DOTween.ToAlpha(() => ui.color, color => ui.color = color, 0.0f, duration);
        // sequenceに追加
        sequence.Append(tween);

        // Easingの設定
        tween.SetEase(Ease.InQuint);
        // 透明から元に戻すTween
        tween = DOTween.ToAlpha(() => ui.color, color => ui.color = color, 1.0f, duration);
        // sequenceに追加
        sequence.Append(tween);

        // 待ち時間を追加
        sequence.PrependInterval(appendInterval);
        // ループの設定
        sequence.SetLoops(-1, LoopType.Restart);

        sequence.Play();
    }

    // Nullチェックを行う
    private void NullCheck()
    {
        switch (uiType)
        {
            case UIType.Text:
                ui = GetComponent<Text>();
                break;

            case UIType.TextMeshPro:
                ui = GetComponent<TextMeshProUGUI>();
                break;

            case UIType.Image:
                ui = GetComponent<Image>();
                break;
        }

        if (ui == null)
        {
            Debug.LogError("ui is Null\nPlease check uiType settings");
        }
    }
}
