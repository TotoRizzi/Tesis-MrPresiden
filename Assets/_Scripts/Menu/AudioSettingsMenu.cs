using UnityEngine;
using UnityEngine.UI;
public class AudioSettingsMenu : MonoBehaviour
{
    [SerializeField] Slider _generalSlider, _musicSlider, _sfxSlider;
    public void SetSFX_SettingsVolume()
    {
        Helpers.AudioManager.sfxSource.volume = _sfxSlider.value;
    }
    public void SetMusicVolume()
    {
        Helpers.AudioManager.musicSource.volume = _musicSlider.value;
    }
    public void SetGeneralVolume()
    {
        AudioListener.volume = _generalSlider.value;
    }
}
