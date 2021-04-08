using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// imageの範囲内にマウスポインタ∸が入った際にSEを鳴らす
/// buttonをクリックした際にSEを鳴らす
/// </summary>
public class ButtonEventSE : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private AudioSource mouseOverSE = null;
    [SerializeField]
    private AudioSource menuHitSE = null;

    private bool isUseDefaultSE = true;

    private void Start()
    {
        if (isUseDefaultSE)
        {
            mouseOverSE = GameObject.FindGameObjectWithTag("MouseOverSE").GetComponent<AudioSource>();
            menuHitSE = GameObject.FindGameObjectWithTag("MenuHitSE").GetComponent<AudioSource>();
        }
        NullCheck();

        GetComponent<Button>().onClick.AddListener(OnPlayMenuHitSE);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOverSE.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }

    public void OnPlayMenuHitSE()
    {
        menuHitSE.Play();
    }

    private void NullCheck()
    {
        mouseOverSE.IsNull();
        menuHitSE.IsNull();
    }
}
