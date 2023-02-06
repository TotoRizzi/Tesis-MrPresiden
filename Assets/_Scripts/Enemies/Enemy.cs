using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour, IDamageable
{
    protected GameManager gameManager;

    public event Action OnUpdate;

    [Header("Health")]
    [SerializeField] float _maxHp = 3;
    float currentHp;

    public virtual void Start()
    {
        gameManager = GameManager.instance;

        gameManager.EnemyManager.AddEnemy(this);
        currentHp = _maxHp;
    }

    public virtual void Update()
    {
        OnUpdate?.Invoke();
    }

    public virtual void TakeDamage(float dmg)
    {
        currentHp -= dmg;
        if (currentHp <= 0) Die();
    }

    public virtual void Die()
    {
        gameManager.EnemyManager.RemoveEnemy(this);
        gameManager.EffectsManager.HumanoindEnemyKilled(transform.position);

        Destroy(gameObject);
    }
}
