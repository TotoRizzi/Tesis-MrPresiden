using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] Image _healthBar = null;
    [SerializeField] Image _staminaBar = null;

    [Header("Combo")]
    [SerializeField] Image _comboBar = null;
    [SerializeField] TextMeshProUGUI _comboCountText;

    [Header("Points")]
    [SerializeField] TextMeshProUGUI _pointsText;
    [SerializeField] TextMeshProUGUI _nextAchievementPointsText;

    [Header("CurrentWeapon")]
    [SerializeField] Image _currentWeaponImg;
    [SerializeField] TextMeshProUGUI _currentWeaponAmmoAmount;

    [Header("Curtain")]
    [SerializeField] Animator _curainAnim;

    [Header("Pause")]
    [SerializeField] GameObject _pauseMenu;

    public void UpdateStaminaBar(float current, float max)
    {
        _staminaBar.fillAmount = current / max;
    }

    public void UpDateComboBar(float current, float max, float currentComboCount)
    {
        _comboBar.fillAmount = current / max;
        _comboCountText.text = currentComboCount.ToString();
    }

    public void UpdatePoints(float points)
    {
        _pointsText.text = points.ToString();
    }

    public void UpdateNextAchievementPoints(float points)
    {
        _nextAchievementPointsText.text = points.ToString();
    }

    public void UpdateHealthBar(float current, float max)
    {
        _healthBar.fillAmount = current / max;
    }

    public void UpdateCurrentWeapon(Sprite currentWeaponSprite)
    {
        _currentWeaponImg.sprite = currentWeaponSprite;
    }
    public void UpdateCurrentAmmo(float currentAmmoAmount)
    {
        _currentWeaponAmmoAmount.text = currentAmmoAmount.ToString();
    }
    public void CloseCurtain()
    {
        if(_curainAnim != null)_curainAnim.Play("Close");
    }

    public void ShowPauseMenu()
    {
        _pauseMenu.SetActive(true);
    }
    public void HidePauseMenu()
    {
        _pauseMenu.SetActive(false);
    }
}
