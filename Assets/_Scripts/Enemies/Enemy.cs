using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour, IDamageable
{
    GameManager _gameManager;

    public event Action OnUpdate;

    [Header("Health")]
    [SerializeField] float _maxHp = 3;
    float currentHp;

    public virtual void Start()
    {
        _gameManager = GameManager.instance;

        _gameManager.EnemyManager.AddEnemy(this);
        currentHp = _maxHp;
    }

    public virtual void Update()
    {
        if (OnUpdate != null) OnUpdate();
    }

    public virtual void TakeDamage(float dmg)
    {
        currentHp -= dmg;
        if (currentHp <= 0) Die();
    }

    public virtual void Die()
    {
        _gameManager.EnemyManager.RemoveEnemy(this);
        _gameManager.EffectsManager.HumanoindEnemyKilled(transform.position);

        Destroy(gameObject);
    }
}
