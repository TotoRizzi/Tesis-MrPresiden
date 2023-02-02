using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    [Header("Combo")]
    [SerializeField] Image _comboBar = null;
    [SerializeField] TextMeshProUGUI _comboCountText;

    public void UpDateComboBar(float current, float max, float currentComboCount)
    {
        _comboBar.fillAmount = current / max;
        _comboCountText.text = currentComboCount.ToString();
    }
}
