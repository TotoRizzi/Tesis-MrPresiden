using UnityEngine;
using Weapons;
public class FireWeapon : Weapon
{
    protected Transform _bulletSpawn;
    [SerializeField] protected int _currentAmmo;
    protected override void Start()
    {
        base.Start();
        _bulletSpawn = transform.GetChild(0);
        _currentAmmo = _weaponData.initialAmmo;
    }
    public override void WeaponAction(Vector2 bulletDirection)
    {
        FireWeaponShoot(bulletDirection);
    }

    protected virtual void FireWeaponShoot(Vector2 bulletDirection)
    {
        if (_currentAmmo <= 0) return;

        _currentAmmo--;

        FRY_Bullet.Instance.pool.GetObject().
                                            SetDmg(_weaponData.damage).
                                            SetSpeed(_weaponData.bulletSpeed).
                                            SetLayer(Layers.PlayerAttack).
                                            SetPosition(_bulletSpawn.position).
                                            SetDirection(bulletDirection);
    }
}
