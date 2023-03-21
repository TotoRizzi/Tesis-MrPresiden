using UnityEngine;
using Weapons;
public class FireWeapon : Weapon
{
    protected Transform _bulletSpawn;
    protected int _currentAmmo;

    public int GetCurrentAmmo { get { return _currentAmmo; } set { _currentAmmo = value; } }
    protected override void Awake()
    {
        base.Awake();
        _currentAmmo = _weaponData.initialAmmo;
    }
    protected override void Start()
    {
        base.Start();
        _bulletSpawn = transform.GetChild(0);
    }
    public override void WeaponAction(Vector2 bulletDirection)
    {
        if (_currentAmmo <= 0)
        {
            Helpers.AudioManager.PlaySFX("Not_Ammo");
            return;
        }

        _currentAmmo--;
        UpdateAmmoAmount();
        Helpers.AudioManager.PlaySFX(_weaponData.weaponSoundName);
        FireWeaponShoot(bulletDirection);
    }

    protected virtual void FireWeaponShoot(Vector2 bulletDirection)
    {
        FRY_Bullet.Instance.pool.GetObject().
                                            SetDmg(_weaponData.damage).
                                            SetSpeed(_weaponData.bulletSpeed).
                                            SetLayer(Layers.PlayerAttack).
                                            SetPosition(_bulletSpawn.position).
                                            SetDirection(bulletDirection).
                                            SetDistance(2f);
    }

    public void UpdateAmmoAmount()
    {
        _gameManager.UiManager.UpdateCurrentAmmo(_currentAmmo);
    }

    public void AddAmmo(int amount)
    {
        _gameManager.UiManager.UpdateCurrentAmmo(_currentAmmo += amount);
    }
}
