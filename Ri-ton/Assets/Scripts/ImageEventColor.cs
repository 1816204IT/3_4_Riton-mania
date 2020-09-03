using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// imageの範囲内にマウスポインタ∸が入った際に指定画像の色を変更する
public class ImageEventColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private Color pointerEnterColor;
    [SerializeField]
    private Color pointerExitColor;

    void Start()
    {
        if(image == null)
        {
            Debug.Log("nullを検知");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = pointerEnterColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = pointerExitColor;
    }
}
