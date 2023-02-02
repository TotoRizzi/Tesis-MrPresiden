using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

public class Knife : Weapon
{
    Vector2 _direction;

    float _attackRate = 2;
    float _timerToAttack;
    bool _attacking;
    private void Update()
    {
    }
    public override void WeaponAction(Vector2 bulletDirection)
    {
    }
}
