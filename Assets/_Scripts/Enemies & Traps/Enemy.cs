using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour, IDamageable
{
    protected GameManager gameManager;

    public event Action OnUpdate;

    [SerializeField] protected Transform sprite;
    private Renderer _renderer;
    private float _onHitRedTime = .2f;

    [Header("Health")]
    [SerializeField] float _maxHp = 3;
    float _currentHp;

    public virtual void Start()
    {
        gameManager = GameManager.instance;

        gameManager.EnemyManager.AddEnemy(this);
        _currentHp = _maxHp;
        _renderer = sprite.GetComponent<Renderer>();
        if (!_renderer) _renderer = sprite.GetChild(0).GetComponent<Renderer>();
    }

    public virtual void Update()
    {
        OnUpdate?.Invoke();
    }

    public virtual void TakeDamage(float dmg)
    {
        _currentHp -= dmg;

        if (_currentHp <= 0)
            Die();
        else
            StartCoroutine(ChangeColor());
    }

    public virtual void Die()
    {
        gameManager.EnemyManager.RemoveEnemy(this);
        gameManager.EffectsManager.HumanoindKilled(transform.position);

        Destroy(gameObject);
    }

    IEnumerator ChangeColor()
    {
        _renderer.material.color = Color.red;

        yield return new WaitForSeconds(_onHitRedTime);

        _renderer.material.color = Color.white;
    }

    protected Vector3 DistanceToPlayer()
    {
        return gameManager.Player ? (gameManager.Player.transform.position + Vector3.up) - transform.position : Vector3.zero;
    }

    protected bool CanSeePlayer()
    {
        return gameManager.Player ? !Physics2D.Raycast(transform.position, DistanceToPlayer().normalized, DistanceToPlayer().magnitude, gameManager.BorderLayer) : false;
    }

    protected void ResetHp()
    {
        _currentHp = _maxHp;
    }
}
