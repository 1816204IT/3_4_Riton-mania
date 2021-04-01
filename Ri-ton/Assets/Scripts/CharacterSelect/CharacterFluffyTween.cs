using DG.Tweening;
using UnityEngine;

/// <summary>
/// 曲選択画面でキャラクターがふわふわしている動きで使用している
/// </summary>
public class CharacterFluffyTween : MonoBehaviour
{
    private Sequence sequence;
    private Tween tween = default;
    [SerializeField]
    private float duration = 0.35f;
    [SerializeField]
    private float appendInterval = 1.0f;
    [SerializeField]
    private float moveDistance = 15.0f;

    private float startPosY = default;
    private float endPosY = default;

    private RectTransform rectTransform = null;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        NullCheck();

        startPosY = rectTransform.position.y;
        endPosY = startPosY + moveDistance;

        CreateTween();
    }

    private void CreateTween()
    {
        sequence = DOTween.Sequence();

        // Easingの設定
        tween.SetEase(Ease.Linear);
        // 下へ動くTween
        tween = rectTransform.DOMoveY(endPosY, duration);
        // sequenceに追加
        sequence.Append(tween);

        // Easingの設定
        tween.SetEase(Ease.Linear);
        // 上へ動くTween
        tween = rectTransform.DOMoveY(startPosY, duration);
        // sequenceに追加
        sequence.Append(tween);

        // 待ち時間を追加
        sequence.PrependInterval(appendInterval);
        // ループの設定
        sequence.SetLoops(-1, LoopType.Yoyo);

        sequence.Play();
    }

    private void NullCheck()
    {
        rectTransform.IsNull(nameof(rectTransform));
    }
}
