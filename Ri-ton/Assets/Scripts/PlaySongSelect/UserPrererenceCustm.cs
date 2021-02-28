using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ユーザーオプションクラス
/// </summary>
public class UserPrererenceCustm : MonoBehaviour
{
    private enum SliderType
    {
        SPEED,
        TIMING,
        MUSIC,
        SE,
        MAX
    }

    public struct SettingItem
    {
        public GameObject obj;
        public Image image;
        public Text num;        // 設定項目の数字
        public Text name;       // SPEED,TIMING等の名前
    }

    private SliderType sliderType = SliderType.SPEED;

    [SerializeField]
    private SoundVolumeManager soundVolumeManager = null;
    [SerializeField]
    private Slider slider = null;
    [SerializeField]
    private Text sliderMinValueText = null;
    [SerializeField]
    private GameObject speedSettingObj = null;
    [SerializeField]
    private GameObject timingSettingObj = null;
    [SerializeField]
    private GameObject musicSettingObj = null;
    [SerializeField]
    private GameObject seSettingObj = null;

    private SettingItem speedItem;
    private SettingItem timingItem;
    private SettingItem musicItem;
    private SettingItem seItem;

    private AudioSource mouseOverSE = null;

    void Start()
    {
        mouseOverSE = GameObject.FindGameObjectWithTag("MouseOverSE").GetComponent<AudioSource>();

        if (slider == null || sliderMinValueText == null || mouseOverSE == null || soundVolumeManager == null)
        {
            Debug.Log("nullを検知");
        }

        SettingItemInit(ref speedItem,  ref speedSettingObj);
        SettingItemInit(ref timingItem, ref timingSettingObj);
        SettingItemInit(ref musicItem,  ref musicSettingObj);
        SettingItemInit(ref seItem,     ref seSettingObj);

        // デフォルトはノーツ速度設定
        AlphaOffCircleImageAndText();
        speedItem.image.color   = Color.white;
        speedItem.num.color     = Color.white;
        slider.value = UserPreference._instance._noteSpeedNum * 2;
        // ノーツ速度
        speedItem.num.text = UserPreference._instance._noteSpeedNum.ToString("f1");
        // オフセット
        timingItem.num.text = UserPreference._instance._offsetValueNum.ToString("f1");
        // 曲ボリューム
        float musicVolume = UserPreference._instance._musicVolume;
        musicVolume *= 10;
        musicItem.num.text = musicVolume.ToString("f1");
        // SEボリューム
        float seVolume = UserPreference._instance._seVolume;
        seVolume *= 10;
        seItem.num.text = seVolume.ToString("f1");
    }

    private void SettingItemInit(ref SettingItem item, ref GameObject obj)
    {
        item.obj = obj;
        item.image = obj.GetComponent<Image>();
        item.num = obj.transform.Find("Num").GetComponent<Text>();
        item.name = obj.transform.Find("SettingName").GetComponent<Text>();
    }

    private void Update()
    {
        float scrollValue = Input.GetAxis("Mouse ScrollWheel");
        if (scrollValue < 0.0f)
        {
            if (slider.value < slider.maxValue)
            {
                slider.value++;
            }
        }
        if (scrollValue > 0.0f)
        {
            if (slider.value > 0)
            {
                slider.value--;
            }
        }
    }

    public void OnSliderValueChanged()
    {
        float value = slider.value;

        if (sliderType == SliderType.SPEED)
        {
            // スピードは1.0以上でないといけないのでスライダー値2.0未満がきたら強制的に1.0にする
            if (value < 2.0f)
            {
                slider.value = 2.0f;
                value = 2.0f;
            }

            value /= 2;
            speedItem.num.text = value.ToString("f1");
            UserPreference._instance.ChangeNoteSpeed(value);
        }
        else if (sliderType == SliderType.TIMING)
        {
            value -= 10.0f;
            timingItem.num.text = value.ToString("f1");
            UserPreference._instance.ChangeOffsetValue(value);
        }
        else if (sliderType == SliderType.MUSIC)
        {
            value /= 2.0f;
            musicItem.num.text = value.ToString("f1");
            UserPreference._instance._musicVolume = value / 10.0f;
            soundVolumeManager.ChangeMusicVolume();
        }
        else
        {
            value /= 2.0f;
            seItem.num.text = value.ToString("f1");
            UserPreference._instance._seVolume = value / 10.0f;
            soundVolumeManager.ChangeSEVolume();
        }

        // SEの再生
        mouseOverSE.Play();
    }

    private void AlphaOffCircleImageAndText()
    {
        Color color = Color.white;
        color.a = 0.3f;

        speedItem.image.color   = color;
        timingItem.image.color  = color;
        musicItem.image.color   = color;
        seItem.image.color      = color;

        speedItem.num.color     = color;
        timingItem.num.color    = color;
        musicItem.num.color     = color;
        seItem.num.color        = color;
    }

    #region スライダーのタイプを変更する
    public void ChangeSliderTypeSPEED()
    {
        sliderType = SliderType.SPEED;
        AlphaOffCircleImageAndText();
        sliderMinValueText.text = "0.0";
        speedItem.image.color   = Color.white;
        speedItem.num.color     = Color.white;
        float speedValue = UserPreference._instance._noteSpeedNum;  // 0～10
        slider.value =  speedValue * 2;     // sliderは0～20の範囲なので2倍する
    }

    public void ChangeSliderTypeTIMING()
    {
        sliderType = SliderType.TIMING;
        AlphaOffCircleImageAndText();
        sliderMinValueText.text = "-10.0";
        timingItem.image.color  = Color.white;
        timingItem.num.color    = Color.white;
        float offsetValue = UserPreference._instance._offsetValueNum;   // -10～10
        slider.value = offsetValue + 10.0f; // 0をスライダーの真ん中に持っていきたいので+10する
    }

    public void ChangeSliderTypeMUSIC()
    {
        sliderType = SliderType.MUSIC;
        AlphaOffCircleImageAndText();
        sliderMinValueText.text = "0.0";
        musicItem.image.color   = Color.white;
        musicItem.num.color     = Color.white;
        float volume = UserPreference._instance._musicVolume;   // 0.0f～1.0f
        slider.value = volume * 20.0f;  // sliderは0～20の範囲なので20倍する
    }

    public void ChangeSliderTypeSE()
    {
        sliderType = SliderType.SE;
        AlphaOffCircleImageAndText();
        sliderMinValueText.text = "0.0";
        seItem.image.color  = Color.white;
        seItem.num.color    = Color.white;
        float volume = UserPreference._instance._seVolume;   // 0.0f～1.0f
        slider.value = volume * 20.0f;  // sliderは0～20の範囲なので20倍する
    }
    #endregion
}
