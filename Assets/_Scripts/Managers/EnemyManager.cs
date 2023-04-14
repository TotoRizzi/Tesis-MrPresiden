using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyManager : MonoBehaviour
{
    GameManager _gameManager;
    private List<Enemy> _allEnemies = new List<Enemy>();

    public event Action OnEnemyKilled;

    int _maxEnemies;

    private void Start()
    {
        _gameManager = Helpers.GameManager;

        _gameManager.OnPlayerDead += ResetLevel;
        Helpers.GameManager.OnPlayerDead += () => StartCoroutine(CheckForEmptyLevel());


        StartCoroutine(CheckForEmptyLevel());
    }

    IEnumerator CheckForEmptyLevel()
    {
        yield return new WaitForSeconds(.1f);

        _maxEnemies = _allEnemies.Count;
        if (_maxEnemies == 0) _gameManager.RoomWon();
        else
            _gameManager.UiManager.UpdateEnemyCount(0, _allEnemies.Count);
    }

    public void AddEnemy(Enemy enemy)
    {
        if (_allEnemies.Contains(enemy)) return;

        _allEnemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        if (!_allEnemies.Contains(enemy)) return;

        OnEnemyKilled();
        _allEnemies.Remove(enemy);
        
        Helpers.AudioManager.PlaySFX("Enemy_Dead");
        Helpers.GameManager.UiManager.UpdateEnemyCount(_allEnemies.Count - _maxEnemies, _maxEnemies);

        if (_allEnemies.Count == 0) _gameManager.RoomWon();
    }

    public void ResetLevel()
    {
        _allEnemies.Clear();
    }
}
