using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Turret : Enemy_Shooting
{
    public override void Start()
    {
        base.Start();
        OnAttack += Shoot;
    }

    void Shoot()
    {
        FRY_Bullet.Instance.pool.GetObject().SetPosition(bulletSpawnPosition.position)
                                            .SetDirection(armPivot.right)
                                            .SetDmg(bulletDamage)
                                            .SetLayer(Layers.EnemyAttack)
                                            .SetSpeed(bulletSpeed);
    }
}