using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Image _IMGhealthBar = null;
    [SerializeField] Image _IMGstaminaBar = null;

    [Header("CurrentWeapon")]
    Sprite _notEquipedWeaponSprite;
    [SerializeField] Image _IMGcurrentWeaponImg;
    [SerializeField] TextMeshProUGUI _TXTcurrentWeaponAmmoAmount;

    [Header("Curtain")]
    [SerializeField] Animator _ANIMcurainAnim;

    [Header("Pause")]
    [SerializeField] GameObject _GOpauseMenu;

    [Header("GameTime")]
    [SerializeField] TextMeshProUGUI _TXTdeathCount;

    private void Awake()
    {
        _notEquipedWeaponSprite = _IMGcurrentWeaponImg.sprite;
    }

    public void UpdateDeathCount(int current)
    {
        _TXTdeathCount.text = current.ToString();
    }

    public void UpdateStaminaBar(float current, float max)
    {
        _IMGstaminaBar.fillAmount = current / max;
    }

    public void UpdateHealthBar(float current, float max)
    {
        _IMGhealthBar.fillAmount = current / max;
    }

    public void UpdateCurrentWeapon(Sprite currentWeaponSprite)
    {
        _IMGcurrentWeaponImg.sprite = currentWeaponSprite;
    }
    public void SetDefaultWeaponSprite()
    {
        _IMGcurrentWeaponImg.sprite = _notEquipedWeaponSprite;
        _TXTcurrentWeaponAmmoAmount.text = 0.ToString();
    }
    public void UpdateCurrentAmmo(float currentAmmoAmount)
    {
        _TXTcurrentWeaponAmmoAmount.text = currentAmmoAmount.ToString();
    }
    public void CloseCurtain()
    {
        if(_ANIMcurainAnim != null)_ANIMcurainAnim.Play("Close");
    }

    public void ShowPauseMenu()
    {
        _GOpauseMenu.SetActive(true);
    }
    public void HidePauseMenu()
    {
        _GOpauseMenu.SetActive(false);
    }
}
