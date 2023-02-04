using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    [Header("Combo")]
    [SerializeField] Image _comboBar = null;
    [SerializeField] TextMeshProUGUI _comboCountText;

    [Header("Points")]
    [SerializeField] TextMeshProUGUI _pointsText;
    [SerializeField] TextMeshProUGUI _nextAchievementPointsText;

    public void UpDateComboBar(float current, float max, float currentComboCount)
    {
        _comboBar.fillAmount = current / max;
        _comboCountText.text = currentComboCount.ToString();
    }

    public void UpdatePoints(float points)
    {
        Debug.Log("Current Points " + points);
        _pointsText.text = points.ToString();
    }

    public void UpdateNextAchievementPoints(float points)
    {
        _nextAchievementPointsText.text = points.ToString();
    }
}
