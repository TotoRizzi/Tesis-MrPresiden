using UnityEngine;
using System.Linq;
using Weapons;
public class Granade : Weapon
{
    float _triggerForce = .5f;
    bool _isActivated;
    public override void WeaponAction(Vector2 bulletDirection)
    {
        ThrowOut(Vector2.zero * 3);
        SetParent(null);
        _rb.AddForce(bulletDirection * _weaponData.throwForce, ForceMode2D.Impulse);
        _isActivated = true;
        _spriteRenderer.sprite = _weaponData.granadeActivatedSprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude >= _triggerForce && _isActivated) Explosion();
    }
    void Explosion()
    {
        var collisions = Physics2D.CircleCastAll(transform.position, _weaponData.explosionRadius, Vector2.one, GameManager.instance.DynamicBodiesLayer).
                                                                                                                                            Where(x => x.collider.GetComponent<Rigidbody2D>() != null).
                                                                                                                                            Select(x => x.collider.GetComponent<Rigidbody2D>()).Where(x => !x.GetComponent<Player>());
        if (collisions.Count() <= 0) return;

        foreach (var item in collisions)
            item.AddExplosionForce(_weaponData.explosionForce, transform.position, _weaponData.explosionRadius);

        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _weaponData.explosionRadius);
    }
}
