using System.Collections;
using UnityEngine;
using Weapons;

public class Knife : Weapon
{
    Vector3 _direction;
    bool _go;
    bool _attack;
    private void Update()
    {
        Vector3 direction = _go ? _direction : _weaponManager.WeaponContainer.position - transform.position;

        if (_attack) transform.position += direction.normalized * _weaponData.knifeSpeed * Time.deltaTime;
    }
    public override void WeaponAction(Vector2 bulletDirection)
    {
        StartCoroutine(Boomerang());
        _direction = bulletDirection;
    }
    IEnumerator Boomerang()
    {
        _attack = _go = true;
        yield return new WaitForSeconds(_weaponData.attackRange);
        _go = false;
        yield return new WaitUntil(() => Vector2.Distance(_weaponManager.WeaponContainer.position, transform.position) <= .01f);
        _attack = _go;
    }
}
