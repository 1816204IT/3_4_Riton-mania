using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// imageの範囲内にマウスポインタ∸が入った際にSEを鳴らす
// buttonをクリックした際にSEを鳴らす
public class ButtonEventSE : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private AudioSource mouseOverSE = null;
    private AudioSource menuHitSE = null;
    private bool isOnPointerEnter = false;

    private void Start()
    {
        mouseOverSE = GameObject.FindGameObjectWithTag("MouseOverSE").GetComponent<AudioSource>();
        menuHitSE = GameObject.FindGameObjectWithTag("MenuHitSE").GetComponent<AudioSource>();
        if (mouseOverSE == null || menuHitSE == null)
        {
            Debug.Log("nullを検知");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isOnPointerEnter = true;
        mouseOverSE.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOnPointerEnter = false;
    }

    public void OnPlayMenuHitSE()
    {
        menuHitSE.Play();
    }

    public bool _isOnPointerEnter
    {
        get { return isOnPointerEnter; }
    }
}
