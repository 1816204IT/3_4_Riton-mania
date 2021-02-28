using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 透明度を用いた点滅表現を行う
/// TextとImageに対応
/// </summary>
public class BlinkingTween : MonoBehaviour
{
    [SerializeField]
    private bool isText = true;
    [SerializeField]
    private float duration = 0.35f;
    [SerializeField]
    private float appendInterval = 1.0f;

    private Text text = null;
    private Image image = null;

    private float nowAlpha = 1.0f; // 現在の透明度


    void Start()
    {
        NullCheck();
        CreateTween();
    }

    void Update()
    {
        if (isText)
        {
            Color c = text.color;
            text.color = new Color(c.r, c.g, c.b, nowAlpha);
        }
        else
        {
            Color c = image.color;
            image.color = new Color(c.r, c.g, c.b, nowAlpha);
        }
    }

    private void CreateTween()
    {
        Sequence sequence = DOTween.Sequence();
        Tween tween = default;

        // Easingの設定
        tween.SetEase(Ease.OutQuint);
        // 透明にするTween
        tween = DOTween.To(() => nowAlpha, alpah => nowAlpha = alpah, 0.0f, duration);
        // sequenceに追加
        sequence.Append(tween);

        // Easingの設定
        tween.SetEase(Ease.InQuint);
        // 透明から元に戻すTween
        tween = DOTween.To(() => nowAlpha, alpah => nowAlpha = alpah, 1.0f, duration);
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
        if (isText)
        {
            text = GetComponent<Text>();

            if (text == null)
            {
                Debug.LogError("text is Null");
            }
        }
        else
        {
            image = GetComponent<Image>();

            if (image == null)
            {
                Debug.Log("image is Null");
            }
        }
    }
}
