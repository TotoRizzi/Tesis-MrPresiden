using UnityEngine;
using UnityEngine.UI;
public class AudioSettings : MonoBehaviour
{
    [SerializeField] Slider _generalSlider, _musicSlider, _sfxSlider;
    public void SetGeneralVolume()
    {
        AudioListener.volume = _generalSlider.value;
    }
    public void SetMusicVolume()
    {
        Helpers.AudioManager.musicSource.volume = _musicSlider.value;
    }
    public void SetSFXVolume()
    {
        Helpers.AudioManager.sfxSource.volume = _sfxSlider.value;
    }

}
