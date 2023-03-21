using UnityEngine;
using Weapons;
using System.Linq;
public class Knife : Weapon
{
    Animator _anim;
    IDamageable _player;
    [SerializeField] Vector2 _capsuleSize = new Vector2(1.6f, 1.1f);
    protected override void Start()
    {
        base.Start();
        _anim = GetComponent<Animator>();
        _player = GetComponentInParent<IDamageable>();
    }
    public override void WeaponAction(Vector2 bulletDirection)
    {
        _anim.SetTrigger("Attack");
        transform.right = bulletDirection;
        _gameManager.SoundManager.PlaySound(_weaponData.weaponSoundName);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damageable = collision.GetComponent<IDamageable>();
        if (damageable != null) damageable.TakeDamage(_weaponData.damage);
    }
}
