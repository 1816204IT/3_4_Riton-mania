using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スライダーの値に合わせてRectTransformのサイズを変更する
/// ユーザー設定のスライダーで使用しています。
/// </summary>
public class SettingSliderMask : MonoBehaviour
{
    [SerializeField]
    private Slider slider = null;

    private RectTransform rt = null;
    private float sliderWidth = 0.0f;
    private float sliderMaxValue = 0.0f;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        rt = this.GetComponent<RectTransform>();
        NullCheck();
        sliderWidth = slider.GetComponent<RectTransform>().sizeDelta.x;
        sliderMaxValue = slider.GetComponent<Slider>().maxValue;
    }

    private void OnEnable()
    {
        Init();
        OnSliderValueChanged();
    }

    public void OnSliderValueChanged()
    {
        float value = slider.value;
        Vector2 tmpSizeDelta = rt.sizeDelta;
        float deltaX = value / sliderMaxValue * sliderWidth;
        rt.sizeDelta = new Vector2(deltaX, tmpSizeDelta.y);
    }

    private void NullCheck()
    {
        slider.IsNull();
        rt.IsNull();
    }
}