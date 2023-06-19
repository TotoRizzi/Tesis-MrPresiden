using UnityEngine;
using Weapons;
public class FireWeapon : Weapon
{
    protected Transform _bulletSpawn;
    protected int _currentAmmo;
    protected Animator _muzzleFlashAnimator;
    LayerMask _borderMask;
    public int GetCurrentAmmo { get { return _currentAmmo; } set { _currentAmmo = value; } }
    protected override void Awake()
    {
        base.Awake();
        _borderMask = LayerMask.GetMask("Border");
        //_currentAmmo = _weaponData.initialAmmo;
    }
    protected override void Start()
    {
        base.Start();
        _bulletSpawn = transform.GetChild(0);
        _muzzleFlashAnimator = _bulletSpawn.GetChild(0).GetComponent<Animator>();
    }
    public override void WeaponAction(Vector2 bulletDirection)
    {
        //if (_currentAmmo <= 0)
        //{
        //    Helpers.AudioManager.PlaySFX("Not_Ammo");
        //    return;
        //}

        Helpers.LevelTimerManager.StartLevelTimer();
        //_currentAmmo--;
        //UpdateAmmoAmount();
        Helpers.AudioManager.PlaySFX(_weaponData.weaponSoundName);
        bool raycast = Physics2D.Raycast(_weaponManager.MainWeaponContainer.position, _bulletSpawn.position - _weaponManager.MainWeaponContainer.position, Vector2.Distance(_bulletSpawn.position, _weaponManager.MainWeaponContainer.position), _borderMask);

        if (!raycast)
        {
            FireWeaponShoot(bulletDirection);
            _muzzleFlashAnimator.Play("MuzzleFlash");
        }
    }

    protected virtual void FireWeaponShoot(Vector2 bulletDirection)
    {
        FRY_PlayerBullet.Instance.pool.GetObject().
                                            SetDmg(_weaponData.damage).
                                            SetSpeed(_weaponData.bulletSpeed).
                                            SetPosition(_bulletSpawn.position).
                                            SetDirection(bulletDirection);
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
