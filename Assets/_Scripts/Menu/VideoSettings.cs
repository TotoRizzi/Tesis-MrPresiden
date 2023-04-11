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

        if(LanguageManager.Instance.selectedLanguage == Languages.eng)
        {
            languages[0] = "Ingles";
            languages[1] = "Español";
        }
        else
        {
            languages[0] = "English";
            languages[1] = "Spanish";
        }

        _languageDropDown.AddOptions(languages);

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

        if (LanguageManager.Instance.selectedLanguage == Languages.eng)
        {
            languages[0] = "Ingles";
            languages[1] = "Español";
        }
        else
        {
            languages[0] = "English";
            languages[1] = "Spanish";
        }
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
