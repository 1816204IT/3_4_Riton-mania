using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapInfoSettings : MonoBehaviour
{
    private JsonManager jsonManager = null;
    private MusicPlayer musicPlayer = null;

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
    private bool isOffsetChangeMode = true;

    void Start()
    {
        jsonManager = GameObject.FindGameObjectWithTag("JsonManager").GetComponent<JsonManager>();
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();

        if (jsonManager == null || musicPlayer == null
            || offsetGroup.obj == null || offsetGroup.slider == null
            || offsetGroup.inputField == null 
            || bpmGroup.obj == null || bpmGroup.slider == null
            || bpmGroup.inputField == null 
            || diffGroup.obj == null || diffGroup.slider == null
            || diffGroup.inputField == null)
        {
            Debug.Log("nullを検知");
        }

        offsetGroup.slider.value = musicPlayer._offset * 100;
        float textValue = offsetGroup.slider.value / 100.0f;
        offsetGroup.inputField.text = textValue.ToString();

        bpmGroup.slider.value = musicPlayer._BPM * 100;
        textValue = bpmGroup.slider.value / 100.0f;
        bpmGroup.inputField.text = textValue.ToString();

        diffGroup.slider.value = jsonManager.LoadMapData(SelectedMap._instance._musicName, SelectedMap._instance._difficultyName).difficulty;
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
        musicPlayer._offset = offsetGroup.slider.value / 100.0f;
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
        musicPlayer._offset = bpmGroup.slider.value / 100.0f;
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
        musicPlayer._offset = offsetGroup.slider.value / 100.0f;
        float textValue = offsetGroup.slider.value / 100.0f;
        offsetGroup.inputField.text = textValue.ToString();
    }

    public void OnValueChangedBpmSlider()
    {
        musicPlayer._BPM = bpmGroup.slider.value / 100.0f;
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
        MapInfo mapInfo = jsonManager.LoadMapInfo(SelectedMap._instance._musicName);
        mapInfo.offset = (int)offsetGroup.slider.value;
        jsonManager.SaveMapInfo(mapInfo);
    }

    public void SaveBPM()
    {
        MapInfo mapInfo = jsonManager.LoadMapInfo(SelectedMap._instance._musicName);
        mapInfo.bpm = (int)bpmGroup.slider.value;
        jsonManager.SaveMapInfo(mapInfo);
    }

    public void SaveDifficulty()
    {
        var mapData = jsonManager.LoadMapData(SelectedMap._instance._musicName, SelectedMap._instance._difficultyName);
        mapData.difficulty = (int)diffGroup.slider.value;
        jsonManager.SaveNotesData(mapData, SelectedMap._instance._musicName, SelectedMap._instance._difficultyName);
    }

    public bool _isOffsetChangeMode
    {
        get { return isOffsetChangeMode; }
    }
}
