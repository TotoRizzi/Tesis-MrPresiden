using System;
using System.Collections.Generic;
using System.IO;
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
        persistantDataSaved = File.Exists(Application.persistentDataPath + $"/{PERSISTANT_DATA}.json") ? SaveLoadSystem.LoadData(PERSISTANT_DATA) : new PersistantDataSaved();
    }
    public void DeletePersistantData()
    {
        SaveLoadSystem.Delete(PERSISTANT_DATA);
        persistantDataSaved = new PersistantDataSaved();
    }
    private void OnDestroy()
    {
        SavePersistantData();
    }
}

[Serializable]
public class PersistantDataSaved
{
    public int unlockedZones = 0;
    public int currentLevel = 1;
    public int gameMode = 0;
    public bool firstTime = true;
    public int currentDeaths;
    public int totalLevels;

    //Deaths per level
    public List<string> levels = new List<string>();
    public List<int> deaths = new List<int>();
}