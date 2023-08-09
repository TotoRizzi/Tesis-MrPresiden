using UnityEngine;
public abstract class Bullet : MonoBehaviour
{
    protected float _speed;
    protected float _dmg;
    protected Vector3 _direction;
    protected TrailRenderer _trail;

    public TrailRenderer Trail { get { return _trail ? _trail : GetComponent<TrailRenderer>(); } }

    protected virtual void Start()
    {
        _trail = GetComponent<TrailRenderer>();
        Helpers.GameManager.OnPlayerDead += ReturnBullet;
    }

    protected virtual void ReturnBullet()
    {
        _trail.sortingOrder = 0;
        _trail.Clear();
    }
}
