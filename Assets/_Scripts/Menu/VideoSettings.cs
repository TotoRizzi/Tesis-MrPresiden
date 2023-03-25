using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class VideoSettings : MonoBehaviour
{
    Resolution[] _resolutions;
    [SerializeField] TMP_Dropdown _resolutionDropDown, _qualityDropDown, _windowModeDropDown;
    private void Start()
    {
        _resolutions = Screen.resolutions;

        _resolutionDropDown.ClearOptions();
    
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + "x" + _resolutions[i].height;
            options.Add(option);
    
            if (_resolutions[i].width == Helpers.CurrentResolution.width && _resolutions[i].height == Helpers.CurrentResolution.height)
                currentResolutionIndex = i;
        }
        _resolutionDropDown.AddOptions(options);
    
        _resolutionDropDown.value = currentResolutionIndex;
    
        _qualityDropDown.value = QualitySettings.GetQualityLevel();
        _qualityDropDown.RefreshShownValue();
    
        _windowModeDropDown.value = (int)Screen.fullScreenMode;
        _windowModeDropDown.RefreshShownValue();
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SetWindowMode(int screenMode)
    {
        FullScreenMode newMode = screenMode == 0? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Screen.fullScreenMode = newMode;
        Helpers.WindowMode = newMode;
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Helpers.CurrentResolution = resolution;
    }
}
