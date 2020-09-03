using DG.Tweening;
using UnityEngine;

public class MoveTween : MonoBehaviour
{
    private Sequence sequence;
    private Tween tween = default;
    [SerializeField]
    private float duration = 0.35f;
    [SerializeField]
    private float appendInterval = 0.0f;
    private Vector3 startPos = default;
    [SerializeField]
    private RectTransform endPos = default;

    private void Start()
    {
        startPos = startPos = GetComponent<RectTransform>().position;
    }

    public void Move()
    {
        GetComponent<Transform>().position = startPos;
        tween = transform.DOMove(endPos.position, duration);

        // Easingの設定
        tween.SetEase(Ease.OutQuint);

        sequence = DOTween.Sequence();
        sequence.Append(tween);

        sequence.Play();
    }

    public void MoveRevert()
    {
        tween = transform.DOMove(startPos, duration);

        // Easingの設定
        tween.SetEase(Ease.OutQuint);

        sequence = DOTween.Sequence();
        sequence.Append(tween);

        sequence.Play();
    }

    // ------------以下キャラクターセレクトシーンで使用しているコールバック付き関数----------------
    public void MoveCharacter(GameObject moveEndPosObj, PickUpCharacterManager manager)
    {
        endPos = moveEndPosObj.GetComponent<RectTransform>();
        tween = transform.DOMove(endPos.position, duration);

        // Easingの設定
        tween.SetEase(Ease.OutQuint);

        sequence = DOTween.Sequence();
        sequence
            .Append(tween)
            .AppendInterval(appendInterval)
            .AppendCallback(() => manager.OnCharacterMoveComplete());

        sequence.Play();
    }

    public void MoveRevertCharacter(PickUpCharacterManager manager)
    {
        tween = transform.DOMove(startPos, duration);

        // Easingの設定
        tween.SetEase(Ease.OutQuint);

        sequence = DOTween.Sequence();
        sequence
            .Append(tween)
            .AppendInterval(appendInterval)
            .AppendCallback(() => manager.OnCharacterMoveRevertComplete());

        sequence.Play();
    }

    public void MoveBG(PickUpCharacterManager manager)
    {
        tween = transform.DOMove(endPos.position, duration);

        // Easingの設定
        tween.SetEase(Ease.OutQuint);

        sequence = DOTween.Sequence();
        sequence
            .Append(tween)
            .AppendInterval(appendInterval)
            .AppendCallback(() => manager.OnAppearBGComplete());

        sequence.Play();
    }

    public void MoveRevertBG(PickUpCharacterManager manager)
    {
        tween = transform.DOMove(startPos, duration);

        // Easingの設定
        tween.SetEase(Ease.OutQuint);

        sequence = DOTween.Sequence();
        sequence
            .Append(tween)
            .AppendInterval(appendInterval)
            .AppendCallback(() => manager.OnVanishBGComplete());

        sequence.Play();
    }
}
