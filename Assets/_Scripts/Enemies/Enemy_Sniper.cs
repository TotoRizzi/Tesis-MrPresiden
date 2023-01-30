using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sniper : Enemy
{
    GameManager _gameManager;
    [SerializeField] Transform _bulletSpawnPosition;
    [SerializeField] Transform _armPivot;

    [SerializeField] float _bulletSpeed = 10;
    [SerializeField] float _attackSpeed = 2f;
    float _currentAttackSpeed;

    public override void Start()
    {
        base.Start();
        _gameManager = GameManager.instance;
        OnUpdate += Attack;
    }

    void Attack()
    {
        Vector3 dirToLookAt = (_gameManager.Player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dirToLookAt.y, dirToLookAt.x) * Mathf.Rad2Deg;

        _armPivot.eulerAngles = new Vector3(0, 0, angle);

        _currentAttackSpeed += Time.deltaTime;

        if (_currentAttackSpeed > _attackSpeed)
        {
            _currentAttackSpeed = 0;

            FRY_Bullet.Instance.pool.GetObject().SetPosition(_bulletSpawnPosition.position)
                                                .SetRotation(_armPivot.rotation)
                                                .SetDmg(1f)
                                                .SetLayer(Layers.EnemyAttack)
                                                .SetSpeed(_bulletSpeed);
        }
    }
}
