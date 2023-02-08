using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ComboManager : MonoBehaviour
{
    GameManager _gameManager;
    Action OnUpdate;

    float _currentComboCount = 0;

    float _currentComboExpireTime = 0;

    float _currentPoints;
    float _pointsForAchieve;

    bool _updateRunning;

    private void Start()
    {
        _gameManager = GameManager.instance;
        _gameManager.EnemyManager.OnEnemyKilled += EnemyKilled;

        _currentPoints = _gameManager.SaveDataManager.GetFloat("CurrentPoints", 0);
        _pointsForAchieve = _gameManager.PointsForAchievement;

        UpdateUiText();
    }

    private void Update()
    {
        OnUpdate?.Invoke();
    }

    public void EnemyKilled()
    {
        _currentComboCount++;

        _currentComboExpireTime = _gameManager.ComboExpireTime;

        AddPoints(_gameManager.PointsPerEnemy - 1 + _currentComboCount);

        if (!_updateRunning)
        {
            _updateRunning = true;
            OnUpdate += ComboTick;
        }
    }

    void ComboTick()
    {
        _currentComboExpireTime -= Time.deltaTime;

        UpdateComboBar();

        if (_currentComboExpireTime <= 0)
        {
            _updateRunning = false;
            _currentComboCount = 0;
            UpdateComboBar();
            OnUpdate -= ComboTick;
        }
    }

    void AddPoints(float points)
    {
        _currentPoints += points;
        _gameManager.SaveDataManager.SaveFloat("CurrentPoints", _currentPoints);
        UpdateUiText();

        if (_currentPoints >= _pointsForAchieve) GiveAchievement();
    }

    void GiveAchievement()
    {
        //Llamar al gamemanager y darle el achievement al player
        _gameManager.GiveAchievement();
        
        _currentPoints = 0;
        float newNextAchievePoints = _gameManager.PointsForAchievement;
        
        newNextAchievePoints = Mathf.RoundToInt(newNextAchievePoints);
        
        _pointsForAchieve = newNextAchievePoints;
        
        UpdateUiText();
        
        _gameManager.SaveDataManager.SaveFloat("CurrentPoints", 0);
        _gameManager.SaveDataManager.SaveFloat("PointsTillNextAchieve", _pointsForAchieve);
    }

    void UpdateUiText()
    {
        _gameManager.UiManager.UpdateNextAchievementPoints(_pointsForAchieve);
        _gameManager.UiManager.UpdatePoints(_currentPoints);
    }
    void UpdateComboBar()
    {
        _gameManager.UiManager.UpDateComboBar(_currentComboExpireTime, _gameManager.ComboExpireTime, _currentComboCount);
    }
}
