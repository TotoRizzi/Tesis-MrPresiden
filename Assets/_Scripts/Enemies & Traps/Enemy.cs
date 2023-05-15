using System.Collections;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour, IDamageable
{
    protected GameManager gameManager;

    public event Action OnUpdate;

    [SerializeField] protected Transform sprite;
    [SerializeField] protected bool _isRobot = false;
    private SpriteRenderer _renderer;
    private float _onHitRedTime = .2f;

    [Header("Health")]
    [SerializeField] float _maxHp = 3;
    float _currentHp;

    public virtual void Start()
    {
        gameManager = Helpers.GameManager;

        gameManager.OnPlayerDead += ReturnObject;

        gameManager.EnemyManager.AddEnemy(this);

        _currentHp = _maxHp;
        _renderer = sprite.GetComponent<SpriteRenderer>();
        if (!_renderer) _renderer = sprite.GetChild(0).GetComponent<SpriteRenderer>();
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

        ReturnObject();
        gameManager.EffectsManager.EnemyKilled(transform.position, _isRobot);
    }

    IEnumerator ChangeColor()
    {
        _renderer.color = Color.red;

        yield return new WaitForSeconds(_onHitRedTime);

        _renderer.color = Color.white;
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

    public Enemy SetPosition(Vector3 pos)
    {
        transform.position = pos;
        return this;
    }

    public virtual void Reset()
    {
        _currentHp = _maxHp;
        if (gameManager) gameManager.EnemyManager.AddEnemy(this);
    }

    public static void TurnOn(Enemy b)
    {
        b.Reset();
        b.gameObject.SetActive(true);
    }

    public static void TurnOff(Enemy b)
    {
        b.gameObject.SetActive(false);
    }
    public virtual void ReturnObject()
    {
        _renderer.color = Color.white;
    }
}
