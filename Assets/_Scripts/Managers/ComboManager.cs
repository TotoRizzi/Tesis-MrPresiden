using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ComboManager : MonoBehaviour
{
    GameManager _gameManager;
    Action OnUpdate;

    float _currentComboCount = 0;
    public float CurrentComboCount { get { return _currentComboCount; } private set { } }

    float _currentComboExpireTime = 0;

    float _currentPoints;
    int _achievementCount;

    bool _updateRunning;

    private void Start()
    {
        _gameManager = GameManager.instance;
        _gameManager.EnemyManager.OnEnemyKilled += EnemyKilled;
    }

    private void Update()
    {
        if (OnUpdate != null) OnUpdate();
    }

    public void EnemyKilled()
    {
        _currentComboCount++;

        _currentComboExpireTime = _gameManager.ComboExpireTime;

        if (!_updateRunning)
        {
            _updateRunning = true;
            OnUpdate += ComboTick;
        }
    }

    void ComboTick()
    {
        _currentComboExpireTime -= Time.deltaTime;

        //_gameManager.UiManager.UpDateComboBar(_currentComboExpireTime, _gameManager.ComboExpireTime, _currentComboCount);

        if (_currentComboExpireTime <= 0)
        {
            _updateRunning = false;
            _currentComboCount = 0;
            OnUpdate -= ComboTick;
        }
    }
}
