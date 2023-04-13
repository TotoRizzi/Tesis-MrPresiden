using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    protected float _speed;
    protected float _maxDistance = 5;
    protected float _currentDistance;
    protected float _dmg;
    protected Vector3 _direction;

    [SerializeField] protected LayerMask _bulletLayer;
    protected virtual void Start()
    {
        Helpers.GameManager.OnPlayerDead += ReturnBullet;
    }

    protected abstract void ReturnBullet();
}
