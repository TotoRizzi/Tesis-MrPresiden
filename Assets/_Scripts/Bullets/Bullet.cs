using UnityEngine;
public abstract class Bullet : MonoBehaviour
{
    protected float _speed;
    protected float _maxDistance = 5;
    protected float _currentDistance;
    protected float _dmg;
    protected Vector3 _direction;
    protected TrailRenderer _trail;

    [SerializeField] protected GameObject pos;
    [SerializeField] protected LayerMask _bulletLayer;
    protected virtual void Start()
    {
        Helpers.GameManager.OnPlayerDead += ReturnBullet;
        _trail = GetComponent<TrailRenderer>();
    }

    protected virtual void ReturnBullet()
    {
        _trail.Clear();
    }
}
