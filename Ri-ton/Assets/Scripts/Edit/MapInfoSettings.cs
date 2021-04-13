using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 譜面情報を設定するクラス
/// オフセット、BPM、難易度
/// </summary>
public class MapInfoSettings : MonoBehaviour
{
    public bool IsOffsetChangeMode { get; set; } = true;

    [System.Serializable]
    public struct MapInfoGroup
    {
        public GameObject obj;
        public Slider slider;
        public InputField inputField;
    }

    [SerializeField]
    MapInfoGroup offsetGroup;
    [SerializeField]
    MapInfoGroup bpmGroup;
    [SerializeField]
    MapInfoGroup diffGroup;

    private JsonManager jsonManager = null;
    private MusicPlayer musicPlayer = null;

    void Start()
    {
        jsonManager = GameObject.FindGameObjectWithTag("JsonManager").GetComponent<JsonManager>();
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        NullCheck();

        offsetGroup.slider.value = musicPlayer.Offset * 100;
        float textValue = offsetGroup.slider.value / 100.0f;
        offsetGroup.inputField.text = textValue.ToString();

        bpmGroup.slider.value = musicPlayer.Bpm * 100;
        textValue = bpmGroup.slider.value / 100.0f;
        bpmGroup.inputField.text = textValue.ToString();

        diffGroup.slider.value = jsonManager.LoadMapData(SelectedMap.Instance.MusicName, SelectedMap.Instance.DifficultyName).difficulty;

        textValue = diffGroup.slider.value / 10.0f;
        diffGroup.inputField.text = textValue.ToString();

        ToggleOffsetChangeMode();
    }

    public void OnEndEditOffsetInputField()
    {
        float max = offsetGroup.slider.maxValue;
        float min = offsetGroup.slider.minValue;
        float num = float.Parse(offsetGroup.inputField.text) * 100.0f;
        if ((num < min) || (num > max))
        {
            return;
        }

        offsetGroup.slider.value = num; ;
        musicPlayer.Offset = offsetGroup.slider.value / 100.0f;
    }

    public void OnEndEditBpmInputField()
    {
        float max = bpmGroup.slider.maxValue;
        float min = bpmGroup.slider.minValue;
        float num = float.Parse(bpmGroup.inputField.text) * 100.0f;
        if ((num < min) || (num > max))
        {
            return;
        }

        bpmGroup.slider.value = num;
        musicPlayer.Offset = bpmGroup.slider.value / 100.0f;
    }

    public void OnEndEditDifficultyInputField()
    {
        float max = diffGroup.slider.maxValue;
        float min = diffGroup.slider.minValue;
        float num = float.Parse(diffGroup.inputField.text) * 10.0f;
        if ((num < min) || (num > max))
        {
            return;
        }

        diffGroup.slider.value = num;
    }

    public void OnValueChangedOffsetSlider()
    {
        musicPlayer.Offset = offsetGroup.slider.value / 100.0f;
        float textValue = offsetGroup.slider.value / 100.0f;
        offsetGroup.inputField.text = textValue.ToString();
    }

    public void OnValueChangedBpmSlider()
    {
        musicPlayer.Bpm = bpmGroup.slider.value / 100.0f;
        float textValue = bpmGroup.slider.value / 100.0f;
        bpmGroup.inputField.text = textValue.ToString();
    }

    public void OnValueChangedDifficultySlider()
    {
        float textValue = diffGroup.slider.value / 10.0f;
        diffGroup.inputField.text = textValue.ToString();
    }

    public void ToggleOffsetChangeMode()
    {
        IsOffsetChangeMode = !IsOffsetChangeMode;
        if (IsOffsetChangeMode)
        {
            offsetGroup.obj.SetActive(true);
            bpmGroup.obj.SetActive(true);
            diffGroup.obj.SetActive(true);
        }
        else
        {
            offsetGroup.obj.SetActive(false);
            bpmGroup.obj.SetActive(false);
            diffGroup.obj.SetActive(false);
        }
    }

    public void SaveOffset()
    {
        MapInfo mapInfo = jsonManager.LoadMapInfo(SelectedMap.Instance.MusicName);
        mapInfo.offset = (int)offsetGroup.slider.value;
        jsonManager.SaveMapInfo(mapInfo);
    }

    public void SaveBPM()
    {
        MapInfo mapInfo = jsonManager.LoadMapInfo(SelectedMap.Instance.MusicName);
        mapInfo.bpm = (int)bpmGroup.slider.value;
        jsonManager.SaveMapInfo(mapInfo);
    }

    public void SaveDifficulty()
    {
        var mapData = jsonManager.LoadMapData(SelectedMap.Instance.MusicName, SelectedMap.Instance.DifficultyName);
        mapData.difficulty = (int)diffGroup.slider.value;
        jsonManager.SaveNoteData(mapData, SelectedMap.Instance.MusicName, SelectedMap.Instance.DifficultyName);
    }

    private void NullCheck()
    {
        jsonManager.IsNull();
        musicPlayer.IsNull();
        offsetGroup.obj.IsNull();
        offsetGroup.slider.IsNull();
        offsetGroup.inputField.IsNull();
        bpmGroup.obj.IsNull();
        bpmGroup.slider.IsNull();
        bpmGroup.inputField.IsNull();
        diffGroup.obj.IsNull();
        diffGroup.slider.IsNull();
        diffGroup.inputField.IsNull();
    }
}
