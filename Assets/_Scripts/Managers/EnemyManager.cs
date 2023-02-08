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
        _gameManager = GameManager.instance;
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
        _gameManager.SoundManager.PlaySound("Enemy_Dead");
    }
}
