using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

// 透明度を用いた文字の点滅
public class BlinkingTween : MonoBehaviour
{
    private Sequence sequence;
    private Tween tween = default;
    [SerializeField]
    private float duration = 0.35f;
    [SerializeField]
    private float appendInterval = 1.0f;

    private Text text = null;
    private float nowAlpha = 1.0f;

    void Start()
    {
        text = GetComponent<Text>();

        if (text == null)
        {
            Debug.Log("nullを検知");
        }

        CreateTween();
    }

    private void CreateTween()
    {
        sequence = DOTween.Sequence();

        // Easingの設定
        tween.SetEase(Ease.OutQuint);
        // 透明にするTween
        tween = DOTween.To( () => nowAlpha, alpah => nowAlpha = alpah, 0.0f, duration);
        // sequenceに追加
        sequence.Append(tween);

        // Easingの設定
        tween.SetEase(Ease.InQuint);
        // 透明から元に戻すTween
        tween = DOTween.To( () => nowAlpha, alpah => nowAlpha = alpah, 1.0f, duration);
        // sequenceに追加
        sequence.Append(tween);

        // 待ち時間を追加
        sequence.PrependInterval(appendInterval);
        // ループの設定
        sequence.SetLoops(-1, LoopType.Restart);

        sequence.Play();
    }

    // Update is called once per frame
    void Update()
    {
        Color c = text.color;
        text.color = new Color(c.r, c.g, c.b, nowAlpha);
    }
}
