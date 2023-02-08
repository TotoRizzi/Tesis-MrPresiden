using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SpawnerFollowDrone : Enemy
{
    [Header("Spawner")]
    [SerializeField] float _spawnTime = 3f;
    float _currentSpawnTime;

    public override void Start()
    {
        base.Start();
        OnUpdate += Spawn;
    }
    void Spawn()
    {
        _currentSpawnTime += Time.deltaTime;

        if (_currentSpawnTime > _spawnTime)
        {
            FRY_FollowDrone.Instance.pool.GetObject().SetPosition(transform.position);
            _currentSpawnTime = 0;
        }
    }
}
