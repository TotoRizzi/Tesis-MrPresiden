using UnityEngine;
using TMPro;
public class EquipableUI : MonoBehaviour
{
    [SerializeField] Canvas _weaponCanvas;
    [SerializeField] TextMeshProUGUI _keyTxt;
    [SerializeField] TextMeshProUGUI _toEquipTxt;

    const string KEY = "E";
    const string TO_EQUIP = "TO EQUIP";

    private void Start()
    {
        _keyTxt.text = KEY;
        _toEquipTxt.text = TO_EQUIP;

        SetActive(false);
    }

    #region BUILDER

    public EquipableUI SetPosition(Vector2 position)
    {
        transform.position = position;
        return this;
    }

    public EquipableUI SetActive(bool enabled)
    {
        _weaponCanvas.gameObject.SetActive(enabled);
        return this;
    }

    #endregion
}
