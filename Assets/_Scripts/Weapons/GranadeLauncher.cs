using UnityEngine;
public class GranadeLauncher : FireWeapon
{
    protected override void FireWeaponShoot(Vector2 bulletDirection)
    {
        var granade = FRY_Granades.Instance.pool.GetObject().
                                            SetDamage(_weaponData.damage).
                                            SetLayer(Layers.PlayerAttack).
                                            SetPosition(_bulletSpawn.position).
                                            SetDirection(bulletDirection);
        granade.ThrowGranade();
    }
}
