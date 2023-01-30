using UnityEngine;
using Weapons;
public class Granade : Weapon
{
    [SerializeField] float _throwForce;

    bool _isActivated;
    public override void WeaponAction(Vector2 bulletDirection)
    {
        ThrowOut(Vector2.zero * 3);
        SetParent(null);
        _rb.AddForce(bulletDirection * _throwForce, ForceMode2D.Impulse);
        _isActivated = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && _isActivated) Destroy(gameObject); 
    }
}
