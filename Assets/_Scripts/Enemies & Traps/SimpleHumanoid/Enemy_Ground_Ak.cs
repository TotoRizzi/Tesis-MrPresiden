using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ground_Ak : Enemy_GroundHumanoid
{
    [SerializeField] Transform _bulletSpawnPosition;
    [SerializeField] Transform _armPivot;
    [SerializeField] Transform _fakeArm;
    [SerializeField] Transform _weaponSprite;

    [SerializeField] float _bulletDamage = 1f;
    [SerializeField] float _bulletSpeed = 10f;
    [SerializeField] float _attackSpeed = 2f;
    float _currentAttackSpeed;

    public override void OnPatrolStart()
    {
        _anim.Play("Run");
        sprite.right = Vector3.right;

    }

    public override void OnAttackStart()
    {
        _anim.Play("Idle");
        _armPivot.gameObject.SetActive(true);
        _fakeArm.gameObject.SetActive(false);
    }

    public override void OnCancelAttack()
    {
        _armPivot.gameObject.SetActive(false);
        _fakeArm.gameObject.SetActive(true);
    }

    public override void OnAttack()
    {
        LookAtPlayer();

        _currentAttackSpeed += Time.deltaTime;

        if (_currentAttackSpeed > _attackSpeed)
        {
            Shoot();
            _currentAttackSpeed = 0;
        }
    }

    void Shoot()
    {
        FRY_Bullet.Instance.pool.GetObject().SetPosition(_bulletSpawnPosition.position)
                                            .SetDirection(_armPivot.right)
                                            .SetDmg(_bulletDamage)
                                            .SetLayer(Layers.EnemyAttack)
                                            .SetSpeed(_bulletSpeed);
    }

    void LookAtPlayer()
    {
        Vector3 dirToLookAt = (gameManager.Player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dirToLookAt.y, dirToLookAt.x) * Mathf.Rad2Deg;

        _armPivot.eulerAngles = new Vector3(0, 0, angle);

        Vector3 newWeaponLocalScale = Vector3.one;
        Vector3 newScale = transform.right;

        if (angle > 90 || angle < -90)
        {
            newScale = -transform.right;
            newWeaponLocalScale.y = -1;
        }
        else
        {
            newScale = transform.right;
            newWeaponLocalScale.x = 1;
        }

        _weaponSprite.localScale = newWeaponLocalScale;
        sprite.right = newScale;
    }

    public override void ReturnObject()
    {
        base.ReturnObject();
        FRY_Enemy_Ground_Ak.Instance.pool.ReturnObject(this);
    }
}
