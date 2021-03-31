using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

/// <summary>
/// 透明度を用いた点滅表現
/// Imageに対応
/// </summary>
public class ColorTween : MonoBehaviour
{
    private Tween tween = null;
    [SerializeField]
    private float duration = 0.1f;
    private float alpha = 0.3f;
    private Image image = null;

    void Start()
    {
        image = GetComponent<Image>();
        NullCheck();
        Color c = image.color;
        AlphaSet(ref c);
        image.color = c;
    }

    public void PlayColorTween(Color changeColor)
    {
        AlphaSet(ref changeColor);

        tween = DOTween.To(
            () => image.color,
            color => image.color = color,
            changeColor,
            duration);

        tween.Play();
    }

    private void AlphaSet(ref Color color)
    {
        Color c = color;
        color = new Color(c.r, c.g, c.b, alpha);
    }

    private void NullCheck()
    {
        image.IsNull(nameof(image));
    }
}
