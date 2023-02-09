using UnityEngine;
using PickUps;
public class AmmoPickUp : PickUp
{
    [SerializeField] int amount;

    WeaponManager _weaponManager;
    protected override void Start()
    {
        base.Start();
        amount = Random.Range(10, 30);
        _uiText = "x" + amount;
        _weaponManager = FindObjectOfType<WeaponManager>();
    }
    public override void PickUpAction()
    {
        _weaponManager.GetMainWeapon.AddAmmo(amount);
        base.PickUpAction();
    }
}
