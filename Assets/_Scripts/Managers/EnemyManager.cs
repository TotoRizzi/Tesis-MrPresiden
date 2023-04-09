using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyManager : MonoBehaviour
{
    GameManager _gameManager;
    private List<Enemy> _allEnemies = new List<Enemy>();

    public event Action OnEnemyKilled;

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

        if (_allEnemies.Count == 0) _gameManager.RoomWon();
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

        if (_allEnemies.Count == 0) _gameManager.RoomWon();
    }

    public void ResetLevel()
    {
        _allEnemies.Clear();
    }
}
