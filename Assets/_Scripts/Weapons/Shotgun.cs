using UnityEngine;
public class Shotgun : FireWeapon
{
    [SerializeField] Transform[] _directions;
    protected override void FireWeaponShoot(Vector2 bulletDirection)
    {
        for (int i = 0; i <= _directions.Length - 1; i++)
        {
            var bullet = FRY_Bullet.Instance.pool.GetObject().
                                                SetDirection(_directions[i].position - _bulletSpawn.position).
                                                SetDmg(_weaponData.damage).
                                                SetLayer(Layers.PlayerAttack).
                                                SetPosition(_bulletSpawn.position).
                                                SetSpeed(_weaponData.bulletSpeed).
                                                SetDistance(.25f);
        }
    }
}
