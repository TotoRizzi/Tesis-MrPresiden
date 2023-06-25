using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WavesGameMode : GameModeManager
{
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] int _maxDeaths = 4;
    int _currentDeaths;

    public override void Start()
    {
        base.Start();
        _currentDeaths = Helpers.GameManager.SaveDataManager.GetInt("WavesCurrentDeaths", 0);
        _text.text = (_maxDeaths - _currentDeaths).ToString();
    }
    public override void PlayerDead()
    {
        playerHealth.EffectsOnDeath();
        playerHealth.RestartPosition();

        _currentDeaths++;
        _text.text = (_maxDeaths - _currentDeaths).ToString();
        Helpers.GameManager.SaveDataManager.SaveInt("WavesCurrentDeaths", _currentDeaths);

        if (_currentDeaths >= _maxDeaths)
            RestartMinigame();
    }

    void RestartMinigame()
    {
        _currentDeaths = 0;
        Helpers.GameManager.SaveDataManager.SaveInt("WavesCurrentDeaths", _currentDeaths);
        Helpers.GameManager.LoadSceneManager.LoadLevel("MiniGame 1 0");
    }
}
