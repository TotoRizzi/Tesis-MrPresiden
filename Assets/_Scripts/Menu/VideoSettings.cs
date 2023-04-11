using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class VideoSettings : MonoBehaviour
{
    Resolution[] _resolutions;
    [SerializeField] TMP_Dropdown _resolutionDropDown, _qualityDropDown, _windowModeDropDown, _languageDropDown;

    List<string> languages = new List<string>() { "English", "Spanish" };
    private void Start()
    {
        #region Language 
        _languageDropDown.ClearOptions();
        _languageDropDown.AddOptions(languages);

        _languageDropDown.value = (int)LanguageManager.Instance.selectedLanguage;

        if(_languageDropDown.value == 0)
        {
            _languageDropDown.options[0].text = "English";
            _languageDropDown.options[1].text = "Spanish";
        }
        else
        {
            _languageDropDown.options[0].text = "Ingles";
            _languageDropDown.options[1].text = "Español";
        }


        #endregion

        #region Resolution

        _resolutions = Screen.resolutions;

        _resolutionDropDown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + "x" + _resolutions[i].height;
            options.Add(option);

            if (_resolutions[i].width == Screen.width && _resolutions[i].height == Screen.height)
                currentResolutionIndex = i;
        }
        _resolutionDropDown.AddOptions(options);

        _resolutionDropDown.value = currentResolutionIndex;

        #endregion

        _qualityDropDown.value = QualitySettings.GetQualityLevel();

        _windowModeDropDown.value = Screen.fullScreenMode == FullScreenMode.FullScreenWindow ? 0 : 1;

        _resolutionDropDown.onValueChanged.AddListener(SetResolution);
        _qualityDropDown.onValueChanged.AddListener(SetQuality);
        _windowModeDropDown.onValueChanged.AddListener(SetWindowMode);
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SetLanguage(int language)
    {
        LanguageManager.Instance.selectedLanguage = (Languages)language;
        LanguageManager.Instance.OnUpdate();

        if (_languageDropDown.value == 0)
        {
            _languageDropDown.options[0].text = languages[0] = "English";
            _languageDropDown.options[1].text = languages[1] = "Spanish";
        }
        else
        {
            _languageDropDown.options[0].text = languages[0] = "Ingles";
            _languageDropDown.options[1].text = languages[1] = "Español";
        }

        _languageDropDown.RefreshShownValue();
    }
    public void SetWindowMode(int screenMode)
    {
        FullScreenMode newMode = screenMode == 0 ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Screen.fullScreenMode = newMode;
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
