using UnityEngine;
using Weapons;
public class Knife : Weapon
{
    Animator _anim;
    protected override void Start()
    {
        base.Start();
        _anim = GetComponent<Animator>();
    }
    public override void WeaponAction(Vector2 bulletDirection)
    {
        _anim.SetTrigger("Attack");
        transform.right = bulletDirection;
        Helpers.AudioManager.PlaySFX(_weaponData.weaponSoundName);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damageable = collision.GetComponent<IDamageable>();
        if (damageable != null) damageable.TakeDamage(_weaponData.damage);
    }
}
