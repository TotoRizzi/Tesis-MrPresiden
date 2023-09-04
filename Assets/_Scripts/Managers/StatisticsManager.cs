using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    GameManager _gameManager;

    int _currentDeathCount;
    string _saveDataName = "CurrentDeathCount";

    private void Start()
    {
        _gameManager = GameManager.instance;
        _currentDeathCount = _gameManager.SaveDataManager.GetInt(_saveDataName, 0);

        _gameManager.OnRoomWon += SaveData;
    }

    void SaveData()
    {
        _gameManager.SaveDataManager.SaveFloat(_saveDataName, _currentDeathCount);
    }

    public void AddDeath()
    {
        _currentDeathCount++;
    }
}
