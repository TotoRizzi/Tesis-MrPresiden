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
    float _pointsForNextAchieve;

    bool _updateRunning;

    private void Start()
    {
        _gameManager = GameManager.instance;
        _gameManager.EnemyManager.OnEnemyKilled += EnemyKilled;

        _currentPoints = _gameManager.SaveDataManager.GetFloat("CurrentPoints", 0);
        _pointsForNextAchieve = _gameManager.SaveDataManager.GetFloat("PointsTillNextAchieve", _gameManager.PointsForFirstAchievement);
        _achievementCount = _gameManager.SaveDataManager.GetInt("AchievementCount", 0);

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

        AddPoints(_gameManager.PointsPerEnemy - 1);

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

        if (_currentPoints >= _pointsForNextAchieve) GiveAchievement();
    }

    void GiveAchievement()
    {
        //Llamar al gamemanager y darle el achievement al player
        //_gameManager.GiveAchievement(_achievementCount);
        //
        //_achievementCount++;
        //_currentPoints = 0;
        //float newNextAchievePoints = _gameManager.PointsForFirstAchievement;
        //for (int i = 0; i < _achievementCount; i++)
        //{
        //    newNextAchievePoints *= _gameManager.AchievementPointsMultiplier;
        //}
        //
        //newNextAchievePoints = Mathf.RoundToInt(newNextAchievePoints);
        //
        //_pointsForNextAchieve = newNextAchievePoints;
        //
        //UpdateUiText();
        //
        //_gameManager.SaveDataManager.SaveInt("AchievementCount", _achievementCount);
        //_gameManager.SaveDataManager.SaveFloat("CurrentPoints", 0);
        //_gameManager.SaveDataManager.SaveFloat("PointsTillNextAchieve", _pointsForNextAchieve);
    }

    void UpdateUiText()
    {
        //_gameManager.UiManager.UpdateNextAchievementPoints(_pointsForNextAchieve);
        _gameManager.UiManager.UpdatePoints(_currentPoints);
    }
    void UpdateComboBar()
    {
        _gameManager.UiManager.UpDateComboBar(_currentComboExpireTime, _gameManager.ComboExpireTime, _currentComboCount);
    }
}
