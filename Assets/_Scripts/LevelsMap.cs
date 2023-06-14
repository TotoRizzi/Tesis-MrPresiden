using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
public class LevelsMap : MonoBehaviour
{
    [SerializeField] Zone[] _zones;
    [SerializeField] int _currentUnlockedZone;
    private void Start()
    {
        _currentUnlockedZone = Helpers.PersistantData.persistantDataSaved.unlockedZones;

        int deathsAmount = 0;
        for (int i = 0; i < _zones[_currentUnlockedZone].levelsZone.Length; i++)
        {
            if (!Helpers.PersistantData.persistantDataSaved.deaths.Any()) break;
            deathsAmount += Helpers.PersistantData.persistantDataSaved.deaths[i + (_zones[i].levelsZone.Length * _zones[_currentUnlockedZone].ID)];
        }

        int actualLevelsZone = 0;
        var coll = _zones.Select(x => x.levelsZone).Take(_currentUnlockedZone);

        foreach (var item in coll)
            actualLevelsZone += item.Count();

        bool canUnlockNewZone = Helpers.PersistantData.persistantDataSaved.levels.Count >= _zones[_currentUnlockedZone].levelsZone.Length  //Chequeo si jugo todos los niveles de la zona
            && deathsAmount <= _zones[_currentUnlockedZone].deathsNeeded  //Chequeo si murio menos veces que lo requerido
            && Helpers.PersistantData.persistantDataSaved.levels.Count >= actualLevelsZone   //Chequeo si jugo mas niveles que los que hay en las zonas desbloqueadas
            && Helpers.PersistantData.persistantDataSaved.unlockedZones < _zones.Length - 1;

        if (canUnlockNewZone) _currentUnlockedZone++;

        for (int i = 0; i <= _currentUnlockedZone; i++)
        {
            _zones[i].zoneButton.interactable = true;
            _zones[i].SetDeathsText();
        }
    }
    private void OnDestroy()
    {
        Helpers.PersistantData.persistantDataSaved.unlockedZones = _currentUnlockedZone;
    }
}

[System.Serializable]
public class Zone
{
    public Button zoneButton;
    public TextMeshProUGUI deathsZoneTxt;
    public string[] levelsZone;
    public int deathsNeeded;
    public int ID;
    public void SetDeathsText()
    {
        int deathsAmount = default;
        for (int i = 0; i < levelsZone.Length; i++)
        {
            var index = Helpers.PersistantData.persistantDataSaved.levels.IndexOf(levelsZone[i]);
            deathsAmount += index < 0 || index > Helpers.PersistantData.persistantDataSaved.deaths.Count ? default : Helpers.PersistantData.persistantDataSaved.deaths[index];
        }

        deathsZoneTxt.gameObject.SetActive(true);
        deathsZoneTxt.rectTransform.position = zoneButton.transform.position + Vector3.up;

        deathsZoneTxt.text = $"{deathsAmount} / {deathsNeeded}";
        deathsZoneTxt.color = deathsAmount <= deathsNeeded ? Color.green : Color.red;
    }
}