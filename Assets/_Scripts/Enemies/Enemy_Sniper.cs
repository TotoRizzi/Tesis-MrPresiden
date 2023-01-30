using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sniper : Enemy_Shooting
{
    public override void Start()
    {
        base.Start();
        OnAttack += Shoot;
    }

    public override void Attack()
    {
        LookAtPlayer();
        base.Attack();
    }

    void Shoot()
    {
        FRY_Bullet.Instance.pool.GetObject().SetPosition(bulletSpawnPosition.position)
                                            .SetDirection(armPivot.right)
                                            .SetDmg(bulletDamage)
                                            .SetLayer(Layers.EnemyAttack)
                                            .SetSpeed(bulletSpeed);
    }

    void LookAtPlayer()
    {
        Vector3 dirToLookAt = (gameManager.Player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dirToLookAt.y, dirToLookAt.x) * Mathf.Rad2Deg;

        armPivot.eulerAngles = new Vector3(0, 0, angle);

        Vector3 newWeaponLocalScale = Vector3.one;
        Vector3 newScale = Vector3.one;

        if (angle > 90 || angle < -90)
        {
            newScale.x = -1;
            newWeaponLocalScale.y = -1;

        }
        else
        {
            newScale.x = 1;
            newWeaponLocalScale.x = 1;
        }

        weaponSprite.localScale = newWeaponLocalScale;
        sprite.localScale = newScale;
    }

}