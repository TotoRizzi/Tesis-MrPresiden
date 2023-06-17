using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
public class LevelsMap : MonoBehaviour
{
    [SerializeField] Button[] _zonesButtons;
    [SerializeField] TextMeshProUGUI[] _deathsZoneTxt;
    private void Start()
    {
        ZonesManager zonesManager = ZonesManager.Instance;
        PersistantDataSaved persistantDataSaved = Helpers.PersistantData.persistantDataSaved;

        for (int i = 0; i <= persistantDataSaved.unlockedZones; i++)
            zonesManager.zones[i].SetCurrentDeaths();

        //for (int i = 0; i < _zones[_currentUnlockedZone].levelsZone.Length; i++)
        //{
        //    if (!Helpers.PersistantData.persistantDataSaved.deaths.Any()) break;
        //    deathsAmount += Helpers.PersistantData.persistantDataSaved.deaths[i + (_zones[i].levelsZone.Length * _zones[_currentUnlockedZone].ID)];
        //}

        var coll = zonesManager.zones.Select(x => x.levelsZone).Take(persistantDataSaved.unlockedZones + 1);
        int actualLevelsZone = 0;

        foreach (var item in coll)
            actualLevelsZone += item.Count();

        bool canUnlockNewZone = persistantDataSaved.levels.Any()  //Chequeo si jugo todos los niveles de la zona
            && persistantDataSaved.currentDeaths <= zonesManager.zones[persistantDataSaved.unlockedZones].deathsNeeded  //Chequeo si murio menos veces que lo requerido
            && persistantDataSaved.levels.Count >= actualLevelsZone   //Chequeo si jugo mas niveles que los que hay en las zonas desbloqueadas
            && persistantDataSaved.unlockedZones < zonesManager.zones.Length - 1;

        if (canUnlockNewZone) persistantDataSaved.unlockedZones++;

        int deathsAmount = 0;
        for (int i = 0; i <= persistantDataSaved.unlockedZones; i++)            //UPDATE DE MUERTES Y ZONAS
        {
            deathsAmount += zonesManager.zones[i].currentDeathsInZone;
            _zonesButtons[i].interactable = true;
            zonesManager.zones[i].SetCurrentDeaths();
            SetDeathsText(_deathsZoneTxt[i], _zonesButtons[i], zonesManager.zones[i].deathsNeeded, ref deathsAmount);
        }
    }
    public void SetDeathsText(TextMeshProUGUI deathsZoneTxt, Button buttonZone, int deathsNeeded, ref int deathsAmount)
    {
        deathsZoneTxt.gameObject.SetActive(true);
        deathsZoneTxt.rectTransform.position = buttonZone.transform.position + Vector3.up;

        deathsZoneTxt.text = $"{deathsAmount} / {deathsNeeded}";
        deathsZoneTxt.color = deathsAmount <= deathsNeeded ? Color.green : Color.red;
    }
}