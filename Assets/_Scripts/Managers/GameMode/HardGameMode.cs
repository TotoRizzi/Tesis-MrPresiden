using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardGameMode : GameModeManager
{
    [SerializeField] int _currentLives;
    string _currentLivesName = "CurrentLives";

    public override void Start()
    {
        base.Start();
        _currentLives = Helpers.GameManager.SaveDataManager.GetInt(_currentLivesName, Helpers.GameManager.DefaultHardLives);
    }

    public override void PlayerDead()
    {
        Debug.Log("HardGameMode");

        _currentLives--;

        if (_currentLives <= 0)
        {
            Helpers.GameManager.SaveDataManager.SaveInt(_currentLivesName, Helpers.GameManager.DefaultHardLives);
            Helpers.GameManager.SceneManager.LoadLevel(1);
        }
        else
        {
            Helpers.GameManager.SaveDataManager.SaveInt(_currentLivesName, _currentLives);
            Debug.Log("currentLives " + _currentLives);
        }

        playerHealth.EffectsOnDeath();
        playerHealth.RestartPosition();

    }
}