using DG.Tweening;
using UnityEngine;

public class TextNowTween : MonoBehaviour
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

        if (rectTransform == null)
        {
            Debug.Log("nullを検知");
        }

        startPosY = rectTransform.position.y;
        endPosY = startPosY - moveDistance;

        CreateTween();
    }

    private void CreateTween()
    {
        sequence = DOTween.Sequence();

        // Easingの設定
        tween.SetEase(Ease.OutQuint);
        // 下へ動くTween
        tween = transform.DOMoveY(endPosY, duration);
        // sequenceに追加
        sequence.Append(tween);

        // Easingの設定
        tween.SetEase(Ease.InQuint);
        // 上へ動くTween
        tween = transform.DOMoveY(startPosY, duration);
        // sequenceに追加
        sequence.Append(tween);

        // 待ち時間を追加
        sequence.PrependInterval(appendInterval);
        // ループの設定
        sequence.SetLoops(-1, LoopType.Restart);

        sequence.Play();
    }

    void Update()
    {
        
    }
}
