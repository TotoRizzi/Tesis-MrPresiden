using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
public class LevelsMap : MonoBehaviour
{
    [SerializeField] Zone[] _zones;
    private void Start()
    {
        int deathsAmount = 0;
        for (int i = 0; i < _zones[Helpers.PersistantData.persistantDataSaved.unbloquedZones].levelsZone.Length; i++)
        {
            if (!Helpers.PersistantData.persistantDataSaved.deaths.Any()) break;
            Debug.Log(Helpers.PersistantData.persistantDataSaved.unbloquedZones);
            deathsAmount += Helpers.PersistantData.persistantDataSaved.deaths[i + (_zones[i].levelsZone.Length * _zones[Helpers.PersistantData.persistantDataSaved.unbloquedZones].ID)];
        }

        int actualLevelsZone = 0;
        var coll = _zones.Select(x => x.levelsZone).Take(Helpers.PersistantData.persistantDataSaved.unbloquedZones + 1);

        foreach (var item in coll)
            actualLevelsZone += item.Count();

        bool canUnlockNewZone = Helpers.PersistantData.persistantDataSaved.levels.Count >= _zones[Helpers.PersistantData.persistantDataSaved.unbloquedZones].levelsZone.Length  //Chequeo si jugo todos los niveles de la zona
            && deathsAmount <= _zones[Helpers.PersistantData.persistantDataSaved.unbloquedZones + 1].deathsNeeded  //Chequeo si murio menos veces que lo requerido
            && Helpers.PersistantData.persistantDataSaved.levels.Count >= actualLevelsZone;    //Chequeo si jugo mas niveles que los que hay en las zonas desbloqueadas

        if (canUnlockNewZone) Helpers.PersistantData.persistantDataSaved.unbloquedZones++;
        for (int i = 0; i <= Helpers.PersistantData.persistantDataSaved.unbloquedZones; i++)
        {
            _zones[i].zoneButton.interactable = true;
            //if (i == 0) continue;
            //_zones[i].SetDeathsText();
        }


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
        int deathsAmount = 0;
        for (int i = 0; i < levelsZone.Length; i++)
        {
            Debug.Log(Helpers.PersistantData.persistantDataSaved.levels.IndexOf(levelsZone[i]));
            deathsAmount += Helpers.PersistantData.persistantDataSaved.deaths[Helpers.PersistantData.persistantDataSaved.levels.IndexOf(levelsZone[i])];
        }

        deathsZoneTxt.gameObject.SetActive(true);
        deathsZoneTxt.rectTransform.position = zoneButton.transform.position + Vector3.up;

        deathsZoneTxt.text = $"{deathsAmount} / {deathsNeeded}";
        deathsZoneTxt.color = deathsAmount <= deathsNeeded ? Color.green : Color.red;
    }
}