using UnityEngine;
using TMPro;
public class EquipableUI : MonoBehaviour
{
    [SerializeField] Canvas _weaponCanvas;
    private void Start()
    {
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
