using DG.Tweening;
using UnityEngine;

/// <summary>
/// Easingを用いた移動を行う
/// キャラクター選択画面で使用している
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

    /// <summary>
    /// Tweenの生成
    /// </summary>
    public void CreateTween()
    {
        GetComponent<Transform>().position = startPos;

        tween = transform.DOMove(endPos.position, duration);
        tween.SetEase(Ease.OutQuint);
        sequence = DOTween.Sequence();
        sequence.Append(tween);
        sequence.Play();
    }

    /// <summary>
    /// 逆再生Tweenの生成
    /// </summary>
    public void CreateRevertTween()
    {
        tween = transform.DOMove(startPos, duration);

        tween.SetEase(Ease.OutQuint);
        sequence = DOTween.Sequence();
        sequence.Append(tween);
        sequence.Play();
    }

    // ------------以下キャラクターセレクトシーンで使用しているコールバック関数----------------

    /// <summary>
    /// キャラクターを選択した際に画面中央に移動するTween
    /// </summary>
    /// <param name="moveEndPosObj"></param>
    /// <param name="manager"></param>
    public void MoveCharacter(GameObject moveEndPosObj, PickUpCharacterManager manager)
    {
        endPos = moveEndPosObj.GetComponent<RectTransform>();
        tween = transform.DOMove(endPos.position, duration);
        tween.SetEase(Ease.OutQuint);

        sequence = DOTween.Sequence();
        sequence
            .Append(tween)
            .AppendInterval(appendInterval)
            .AppendCallback(() => manager.OnCharacterMoveComplete());

        sequence.Play();
    }

    /// <summary>
    ///キャラクターを画面中央から定位置に移動するTween
    /// </summary>
    /// <param name="manager"></param>
    public void MoveRevertCharacter(PickUpCharacterManager manager)
    {
        tween = transform.DOMove(startPos, duration);
        tween.SetEase(Ease.OutQuint);

        sequence = DOTween.Sequence();
        sequence
            .Append(tween)
            .AppendInterval(appendInterval)
            .AppendCallback(() => manager.OnCharacterMoveRevertComplete());

        sequence.Play();
    }

    /// <summary>
    /// キャラクターを選択した際に背景の画像をスライドさせるTween
    /// </summary>
    /// <param name="manager"></param>
    public void SlideBG(PickUpCharacterManager manager)
    {
        tween = transform.DOMove(endPos.position, duration);
        tween.SetEase(Ease.OutQuint);

        sequence = DOTween.Sequence();
        sequence
            .Append(tween)
            .AppendInterval(appendInterval)
            .AppendCallback(() => manager.OnAppearBGComplete());

        sequence.Play();
    }

    /// <summary>
    /// キャラクター選択解除時に背景の画像をスライドさせるTween
    /// </summary>
    /// <param name="manager"></param>
    public void RevertSlideBG(PickUpCharacterManager manager)
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
