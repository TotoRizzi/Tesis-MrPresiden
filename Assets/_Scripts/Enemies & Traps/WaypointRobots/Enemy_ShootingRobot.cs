using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ShootingRobot : Enemy_Waypoint
{
    [SerializeField] Transform _bulletSpawnPosition;
    [SerializeField] Transform _armPivot;

    [SerializeField] float _bulletSpeed = 10;
    [SerializeField] float _bulletsPerShot = 3;
    [SerializeField] float _timeBetweenBullets = .2f;
    [SerializeField] float _attackSpeed = 2f;
    float _currentBulletsShot;
    float _currentTimeBetweenBullets;
    float _currentAttackSpeed;

    public override void Start()
    {
        base.Start();
        OnUpdate += Attack;
        OnUpdate += CalculateAttack;
    }

    public void Attack()
    {
        Vector3 dirToLookAt = (gameManager.Player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dirToLookAt.y, dirToLookAt.x) * Mathf.Rad2Deg;

        _armPivot.eulerAngles = new Vector3(0, 0, angle);
    }

    void CalculateAttack()
    {
        _currentAttackSpeed += Time.deltaTime;

        if (_currentAttackSpeed > _attackSpeed)
        {
            _currentAttackSpeed = 0;

            OnUpdate += Shoot;
            OnUpdate -= CalculateAttack;
        }
    }

    void Shoot()
    {

        if(_currentBulletsShot < _bulletsPerShot)
        {
            _currentTimeBetweenBullets += Time.deltaTime;

            if(_currentTimeBetweenBullets > _timeBetweenBullets)
            {
                FRY_Bullet.Instance.pool.GetObject().SetDmg(1f)
                                                    .SetSpeed(_bulletSpeed)
                                                    .SetLayer(Layers.EnemyAttack)
                                                    .SetPosition(_bulletSpawnPosition.position)
                                                    .SetDirection(_armPivot.right);
                _currentTimeBetweenBullets = 0;
                _currentBulletsShot++;
            }
        }
        else
        {
            _currentBulletsShot = 0;

            OnUpdate -= Shoot;
            OnUpdate += CalculateAttack;
        }

    }
}