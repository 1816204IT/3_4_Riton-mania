using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 曲選択の矢印画像が右矢印と左矢印の2個あり
// これら2個のアニメーションを同期させる管理スクリプト
public class ArrowAnimationSynchro : MonoBehaviour
{
    [SerializeField]
    private MusicSelectArrow leftArrow = null;
    [SerializeField]
    private MusicSelectArrow rightArrow = null;

    private bool isOnPointerEnter = false;

    void Start()
    {
        if (leftArrow == null || rightArrow == null)
        {
            Debug.Log("nullを検知");
        }
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
}
