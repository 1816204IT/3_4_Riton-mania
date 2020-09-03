using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

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
        Color c = image.color;
        AlphaSet(ref c);
        image.color = c;

        if (image == null)
        {
            Debug.Log("nullを検知");
        }
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
}
