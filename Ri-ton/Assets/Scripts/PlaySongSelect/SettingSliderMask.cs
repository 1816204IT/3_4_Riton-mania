using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

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

        if (slider == null || rt == null)
        {
            Debug.Log("nullを検知");
        }
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
}