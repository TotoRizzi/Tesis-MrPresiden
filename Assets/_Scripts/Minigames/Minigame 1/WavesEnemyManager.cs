using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WavesEnemyManager : BaseEnemyManager
{
    int _enemiesKilled;

    public event Action OnWaveWon;
    Action OnUpdate;

    [SerializeField] Transform[] _enemiesSpawners;
    List<Enemy> _enemiesAlive;

    [SerializeField] float _timePerSpawn;
    float _currentTimePerSpawn;

    int _enemiesToSpawn;
    int _currentEnemiesSpawned;
    public int currentLevel;


    public override void Start()
    {
        StartCoroutine(Wait());
    }
    private void Update()
    {
        OnUpdate?.Invoke();
    }

    IEnumerator Wait()
    {
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        _enemiesToSpawn = currentLevel + 1;
        OnUpdate += SpawnEnemies;
    }

    void SpawnEnemies()
    {
        if (_currentEnemiesSpawned >= _enemiesToSpawn)
            OnUpdate -= SpawnEnemies;

        _currentTimePerSpawn += Time.deltaTime;

        if (_currentTimePerSpawn >= _timePerSpawn)
            SpawnEnemy();
    }

    void SpawnEnemy()
    {
        _currentTimePerSpawn = 0;
        _currentEnemiesSpawned++;
        FRY_Enemy_FollowDrone.Instance.pool.GetObject().SetPosition(_enemiesSpawners[UnityEngine.Random.Range(0, _enemiesSpawners.Length)].position);
    }

    public override void RemoveEnemy(Enemy enemy)
    {
        if (!_allEnemies.Contains(enemy)) return;

        EnemyKilled();
        _allEnemies.Remove(enemy);
        _enemiesKilled++;

        Helpers.AudioManager.PlaySFX("Enemy_Dead");//tendria que agregarse desde el audiomanager

        if (_enemiesKilled >= _enemiesToSpawn) OnWaveWon?.Invoke();
    }

    public bool EnemiesLeftAlive()
    {
        return _allEnemies.Count == 0 ? false : true;
    }

    public override string EnemyCountString()
    {
        return _enemiesKilled.ToString() + "/ " + _enemiesToSpawn.ToString();
    }
}
