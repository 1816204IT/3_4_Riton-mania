using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ユーザーオプションカスタマイズ
/// </summary>
public class UserPrererenceCustm : MonoBehaviour
{
    private enum SliderType
    {
        SPEED,  // ノーツ落下速度
        TIMING, // タイミングオフセット
        MUSIC,  // 音量
        SE,     // 効果音
        MAX
    }

    public struct SettingItem
    {
        public GameObject obj;
        public Image image;
        public Text num;        // 設定項目の数字
        public Text name;       // SPEED,TIMING等の名前
    }

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
    private SliderType sliderType = SliderType.SPEED;
    private AudioSource mouseOverSE = null;

    void Start()
    {
        mouseOverSE = GameObject.FindGameObjectWithTag("MouseOverSE").GetComponent<AudioSource>();
        NullCheck();

        SettingItemInit(ref speedItem,  ref speedSettingObj);
        SettingItemInit(ref timingItem, ref timingSettingObj);
        SettingItemInit(ref musicItem,  ref musicSettingObj);
        SettingItemInit(ref seItem,     ref seSettingObj);

        // デフォルトはノーツ速度設定
        AlphaOffCircleImageAndText();
        speedItem.image.color   = Color.white;
        speedItem.num.color     = Color.white;
        slider.value = UserPreference.Instance.NoteSpeedNum * 2;
        // ノーツ速度
        speedItem.num.text = UserPreference.Instance.NoteSpeedNum.ToString("f1");
        // オフセット
        timingItem.num.text = UserPreference.Instance.OffsetValueNum.ToString("f1");
        // 曲ボリューム
        float musicVolume = UserPreference.Instance.MusicVolume;
        musicVolume *= 10;
        musicItem.num.text = musicVolume.ToString("f1");
        // SEボリューム
        float seVolume = UserPreference.Instance.SeVolume;
        seVolume *= 10;
        seItem.num.text = seVolume.ToString("f1");
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

    /// <summary>
    /// 設定項目の初期化
    /// </summary>
    /// <param name="obj">初期化対象のGameObject</param>
    private void SettingItemInit(ref SettingItem item, ref GameObject obj)
    {
        item.obj = obj;
        item.image = obj.GetComponent<Image>();
        item.num = obj.transform.Find("Num").GetComponent<Text>();
        item.name = obj.transform.Find("SettingName").GetComponent<Text>();
    }

    /// <summary>
    /// スライダーの値が変更された時の処理
    /// </summary>
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
            UserPreference.Instance.ChangeNoteSpeed(value);
        }
        else if (sliderType == SliderType.TIMING)
        {
            value -= 10.0f;
            timingItem.num.text = value.ToString("f1");
            UserPreference.Instance.ChangeOffsetValue(value);
        }
        else if (sliderType == SliderType.MUSIC)
        {
            value /= 2.0f;
            musicItem.num.text = value.ToString("f1");
            UserPreference.Instance.MusicVolume = value / 10.0f;
            soundVolumeManager.ChangedMusicVolume();
        }
        else
        {
            value /= 2.0f;
            seItem.num.text = value.ToString("f1");
            UserPreference.Instance.SeVolume = value / 10.0f;
            soundVolumeManager.ChangedSEVolume();
        }

        // SEの再生
        mouseOverSE.Play();
    }

    /// <summary>
    /// 画像とテキストの色をグレーアウトする
    /// </summary>
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
        float speedValue = UserPreference.Instance.NoteSpeedNum;  // 0～10
        slider.value =  speedValue * 2;     // sliderは0～20の範囲なので2倍する
    }

    public void ChangeSliderTypeTIMING()
    {
        sliderType = SliderType.TIMING;
        AlphaOffCircleImageAndText();
        sliderMinValueText.text = "-10.0";
        timingItem.image.color  = Color.white;
        timingItem.num.color    = Color.white;
        float offsetValue = UserPreference.Instance.OffsetValueNum;   // -10～10
        slider.value = offsetValue + 10.0f; // 0をスライダーの真ん中に持っていきたいので+10する
    }

    public void ChangeSliderTypeMUSIC()
    {
        sliderType = SliderType.MUSIC;
        AlphaOffCircleImageAndText();
        sliderMinValueText.text = "0.0";
        musicItem.image.color   = Color.white;
        musicItem.num.color     = Color.white;
        float volume = UserPreference.Instance.MusicVolume;   // 0.0f～1.0f
        slider.value = volume * 20.0f;  // sliderは0～20の範囲なので20倍する
    }

    public void ChangeSliderTypeSE()
    {
        sliderType = SliderType.SE;
        AlphaOffCircleImageAndText();
        sliderMinValueText.text = "0.0";
        seItem.image.color  = Color.white;
        seItem.num.color    = Color.white;
        float volume = UserPreference.Instance.SeVolume;   // 0.0f～1.0f
        slider.value = volume * 20.0f;  // sliderは0～20の範囲なので20倍する
    }
    #endregion

    private void NullCheck()
    {
        slider.IsNull();
        sliderMinValueText.IsNull();
        mouseOverSE.IsNull();
        soundVolumeManager.IsNull();
    }
}