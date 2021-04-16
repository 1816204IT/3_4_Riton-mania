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

    /// <summary>
    /// ボタン上にマウスカーソルが入った際の処理
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOverSE.Play();
    }

    /// <summary>
    /// ボタン上からマウスカーソルが出た際の処理
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
    }

    /// <summary>
    /// ボタンフォーカスSE再生
    /// </summary>
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
