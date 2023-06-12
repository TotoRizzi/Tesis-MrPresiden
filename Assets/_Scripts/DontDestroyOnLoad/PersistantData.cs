using System;
using UnityEngine;
public class PersistantData : MonoBehaviour
{
    public event Action savePersistantData = delegate { };
    public PersistantDataSaved persistantDataSaved;

    public const string PERSISTANT_DATA = "Persistant data";
    public static PersistantData Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        LoadPersistantData();
    }
    public void SavePersistantData()
    {
        savePersistantData();
        SaveLoadSystem.SaveData(persistantDataSaved, PERSISTANT_DATA);
    }
    public void LoadPersistantData()
    {
        persistantDataSaved = SaveLoadSystem.LoadData(PERSISTANT_DATA);
    }
    public void DeletePersistantData()
    {
        SaveLoadSystem.Delete(PERSISTANT_DATA);
        persistantDataSaved.ResetValues();
    }
    private void OnDestroy()
    {
        SavePersistantData();
    }
}

[Serializable]
public class PersistantDataSaved
{
    public int unbloquedZones = 0;
    public int currentLevel = 1;
    public int gameMode = 0;
    public bool firstTime = true;

    public void ResetValues()
    {
        unbloquedZones = 0;
        currentLevel = 1;
        gameMode = 0;
        firstTime = true;
    }
}