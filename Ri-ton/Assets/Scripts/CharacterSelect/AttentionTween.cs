using DG.Tweening;
using UnityEngine;

/// <summary>
/// 上下に動くTween
/// </summary>
public class AttentionTween : MonoBehaviour
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
        sequence.Kill();
        sequence = DOTween.Sequence();

        // Easingの設定
        tween.SetEase(Ease.OutQuint);
        // 下へ動くTween
        tween = rectTransform.DOMoveY(endPosY, duration);
        // sequenceに追加
        sequence.Append(tween);

        // Easingの設定
        tween.SetEase(Ease.InQuint);
        // 上へ動くTween
        tween = rectTransform.DOMoveY(startPosY, duration);
        // sequenceに追加
        sequence.Append(tween);

        // 待ち時間を追加
        sequence.PrependInterval(appendInterval);
        // ループの設定
        sequence.SetLoops(-1, LoopType.Restart);

        sequence.Play();
    }

    public void TweenReSetting(float newStartPos)
    {
        if (newStartPos != -1)
        {
            SetStartPosY(newStartPos);
        }
        CreateTween();
    }

    private void SetStartPosY(float value)
    {
        startPosY = value;
        endPosY = startPosY + moveDistance;
    }

    public void MoveDirDown()
    {
        moveDistance = Mathf.Abs(moveDistance) * -1;
    }

    public void MoveDirUp()
    {
        moveDistance = Mathf.Abs(moveDistance);
    }

    private void NullCheck()
    {
        rectTransform.IsNull(nameof(rectTransform));
    }
}