using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    EnemyManager _enemyManager;
    [Header("Health")]
    [SerializeField] float _maxHp = 3;
    float currentHp;

    private void Start()
    {
        _enemyManager = EnemyManager.instance;

        currentHp = _maxHp;
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
    }
}
