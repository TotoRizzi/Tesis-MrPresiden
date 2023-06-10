using UnityEngine;
using UnityEngine.UI;
public class LevelsMap : MonoBehaviour
{
    [SerializeField] Button[] _levelZonesButtons;

    private void Start()
    {
        for (int i = 0; i <= Helpers.PersistantData.persistantDataSaved.unbloquedZones; i++)
            _levelZonesButtons[i].interactable = true;
    }
}
