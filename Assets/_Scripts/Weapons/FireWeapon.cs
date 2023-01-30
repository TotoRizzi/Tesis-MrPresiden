using UnityEngine;
using Weapons;
public class FireWeapon : Weapon
{
    protected Transform _bulletSpawn;
    protected override void Start()
    {
        base.Start();
        _bulletSpawn = transform.GetChild(0);
    }
    public override void WeaponAction(Vector2 bulletDirection)
    {
        FireWeaponShoot(bulletDirection);
    }

    protected virtual void FireWeaponShoot(Vector2 bulletDirection)
    {
        FRY_Bullet.Instance.pool.GetObject().
                                            SetDmg(_weaponData.damage).
                                            SetSpeed(_weaponData.bulletSpeed).
                                            SetLayer(Layers.PlayerAttack).
                                            SetPosition(_bulletSpawn.position).
                                            SetDirection(bulletDirection);
    }
}
