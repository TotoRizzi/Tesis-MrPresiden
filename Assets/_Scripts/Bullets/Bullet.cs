using UnityEngine;
public abstract class Bullet : MonoBehaviour
{
    protected float _speed;
    protected float _dmg;
    protected Vector3 _direction;
    protected TrailRenderer _trail;
    protected virtual void Start()
    {
        _trail = GetComponent<TrailRenderer>();
        Helpers.GameManager.OnPlayerDead += ReturnBullet;
    }

    protected virtual void ReturnBullet()
    {
        _trail.Clear();
    }
}
