using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour, IDamageable
{
    EnemyManager _enemyManager;

    public event Action OnUpdate;

    [Header("Health")]
    [SerializeField] float _maxHp = 3;
    float currentHp;

    public virtual void Start()
    {
        _enemyManager = EnemyManager.instance;
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
        _enemyManager.OnEnemyKilled();
        FRY_EnemyExplodeParticle.Instance.pool.GetObject().SetPosition(transform.position);
        FRY_EnemyBloodSplatter.Instance.pool.GetObject().SetPosition(transform.position);

        Destroy(gameObject);
    }
}
