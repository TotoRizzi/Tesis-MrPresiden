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

        for (int i = 0; i <= _currentUnlockedZone; i++)
            _zones[i].SetCurrentDeaths();

        int deathsAmount = 0;
        //for (int i = 0; i < _zones[_currentUnlockedZone].levelsZone.Length; i++)
        //{
        //    if (!Helpers.PersistantData.persistantDataSaved.deaths.Any()) break;
        //    deathsAmount += Helpers.PersistantData.persistantDataSaved.deaths[i + (_zones[i].levelsZone.Length * _zones[_currentUnlockedZone].ID)];
        //}

        for (int i = 0; i <= _currentUnlockedZone; i++)                  //CUANTAS MUERTES TIENE PARA SABER SI DESBLOQUEA UNA ZONA
        {
            if (!Helpers.PersistantData.persistantDataSaved.deaths.Any()) break;

            deathsAmount += _zones[i].currentDeathsInZone;
        }

        var coll = _zones.Select(x => x.levelsZone).Take(_currentUnlockedZone + 1);
        int actualLevelsZone = 0;

        foreach (var item in coll)
            actualLevelsZone += item.Count();

        bool canUnlockNewZone = Helpers.PersistantData.persistantDataSaved.levels.Any()  //Chequeo si jugo todos los niveles de la zona
            && deathsAmount <= _zones[_currentUnlockedZone].deathsNeeded  //Chequeo si murio menos veces que lo requerido
            && Helpers.PersistantData.persistantDataSaved.levels.Count >= actualLevelsZone   //Chequeo si jugo mas niveles que los que hay en las zonas desbloqueadas
            && Helpers.PersistantData.persistantDataSaved.unlockedZones < _zones.Length - 1;

        if (canUnlockNewZone) _currentUnlockedZone++;

        deathsAmount = 0;
        for (int i = 0; i <= _currentUnlockedZone; i++)            //UPDATE DE MUERTES Y ZONAS
        {
            deathsAmount += _zones[i].currentDeathsInZone;
            _zones[i].zoneButton.interactable = true;
            _zones[i].SetCurrentDeaths();
            _zones[i].SetDeathsText(ref deathsAmount);
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
    public int currentDeathsInZone;
    public int ID;
    public void SetDeathsText(ref int deathsAmount)
    {
        deathsZoneTxt.gameObject.SetActive(true);
        deathsZoneTxt.rectTransform.position = zoneButton.transform.position + Vector3.up;

        deathsZoneTxt.text = $"{deathsAmount} / {deathsNeeded}";
        deathsZoneTxt.color = deathsAmount <= deathsNeeded ? Color.green : Color.red;
    }
    public void SetCurrentDeaths()
    {
        for (int i = 0; i < levelsZone.Length; i++)
        {
            int index = Helpers.PersistantData.persistantDataSaved.levels.IndexOf(levelsZone[i]);
            int deathNum = index < 0 ? 0 : Helpers.PersistantData.persistantDataSaved.deaths[index];
            currentDeathsInZone += deathNum;
        }
    }
}