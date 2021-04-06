using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 譜面情報を設定するクラス
/// オフセット、BPM、難易度
/// </summary>
public class MapInfoSettings : MonoBehaviour
{
    public bool isOffsetChangeMode { get; set; } = true;

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

        offsetGroup.slider.value = musicPlayer.offset * 100;
        float textValue = offsetGroup.slider.value / 100.0f;
        offsetGroup.inputField.text = textValue.ToString();

        bpmGroup.slider.value = musicPlayer.bpm * 100;
        textValue = bpmGroup.slider.value / 100.0f;
        bpmGroup.inputField.text = textValue.ToString();

        diffGroup.slider.value = jsonManager.LoadMapData(SelectedMap.instance.musicName, SelectedMap.instance.difficultyName).difficulty;

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
        musicPlayer.offset = offsetGroup.slider.value / 100.0f;
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
        musicPlayer.offset = bpmGroup.slider.value / 100.0f;
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
        musicPlayer.offset = offsetGroup.slider.value / 100.0f;
        float textValue = offsetGroup.slider.value / 100.0f;
        offsetGroup.inputField.text = textValue.ToString();
    }

    public void OnValueChangedBpmSlider()
    {
        musicPlayer.bpm = bpmGroup.slider.value / 100.0f;
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
        isOffsetChangeMode = !isOffsetChangeMode;
        if (isOffsetChangeMode)
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
        MapInfo mapInfo = jsonManager.LoadMapInfo(SelectedMap.instance.musicName);
        mapInfo.offset = (int)offsetGroup.slider.value;
        jsonManager.SaveMapInfo(mapInfo);
    }

    public void SaveBPM()
    {
        MapInfo mapInfo = jsonManager.LoadMapInfo(SelectedMap.instance.musicName);
        mapInfo.bpm = (int)bpmGroup.slider.value;
        jsonManager.SaveMapInfo(mapInfo);
    }

    public void SaveDifficulty()
    {
        var mapData = jsonManager.LoadMapData(SelectedMap.instance.musicName, SelectedMap.instance.difficultyName);
        mapData.difficulty = (int)diffGroup.slider.value;
        jsonManager.SaveNoteData(mapData, SelectedMap.instance.musicName, SelectedMap.instance.difficultyName);
    }

    private void NullCheck()
    {
        jsonManager.IsNull(nameof(jsonManager));
        musicPlayer.IsNull(nameof(musicPlayer));
        offsetGroup.obj.IsNull(nameof(offsetGroup.obj));
        offsetGroup.slider.IsNull(nameof(offsetGroup.slider));
        offsetGroup.inputField.IsNull(nameof(offsetGroup.inputField));
        bpmGroup.obj.IsNull(nameof(bpmGroup.obj));
        bpmGroup.slider.IsNull(nameof(bpmGroup.slider));
        bpmGroup.inputField.IsNull(nameof(bpmGroup.inputField));
        diffGroup.obj.IsNull(nameof(diffGroup.obj));
        diffGroup.slider.IsNull(nameof(diffGroup.slider));
        diffGroup.inputField.IsNull(nameof(diffGroup.inputField));
    }
}
