using DG.Tweening;
using UnityEngine;

/// <summary>
/// Easingを用いた移動を行う
/// キャラクター選択画面等で使用している
/// </summary>
public class MoveTween : MonoBehaviour
{
    [SerializeField]
    private float duration = 0.0f;
    [SerializeField]
    private float appendInterval = 0.0f;
    [SerializeField]
    private RectTransform endPos = default;

    private Sequence sequence;
    private Tween tween = default;
    private Vector3 startPos = default;

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
