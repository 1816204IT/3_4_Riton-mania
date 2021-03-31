using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 曲選択の矢印画像が右矢印と左矢印の2個あり
/// これら2個のアニメーションを同期させる管理スクリプト
/// </summary>
public class ArrowAnimationSynchro : MonoBehaviour
{
    [SerializeField]
    private MusicSelectArrow leftArrow = null;
    [SerializeField]
    private MusicSelectArrow rightArrow = null;

    void Start()
    {
        NullCheck();
    }

    public void PauseAnimation()
    {
        leftArrow.PauseAnimation();
        rightArrow.PauseAnimation();
    }

    public void PlayAnimation()
    {
        leftArrow.PlayAnimation();
        rightArrow.PlayAnimation();
    }

    private void NullCheck()
    {
        leftArrow.IsNull(nameof(leftArrow));
        rightArrow.IsNull(nameof(rightArrow));
    }
}