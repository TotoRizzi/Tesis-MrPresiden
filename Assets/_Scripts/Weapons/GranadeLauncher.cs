using UnityEngine;
public class GranadeLauncher : FireWeapon
{
    protected override void FireWeaponShoot(Vector2 bulletDirection)
    {
        var granade = FRY_Granades.Instance.pool.GetObject().
                                            SetDamage(_weaponData.damage).
                                            SetPosition(_bulletSpawn.position).
                                            SetDirection(bulletDirection).
                                            SetExplosion(_weaponData.bulletExplosion);
        granade.ThrowGranade();
    }
}
