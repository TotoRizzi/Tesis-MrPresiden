using UnityEngine;
public class EnemyBullet : Bullet
{
    private void FixedUpdate()
    {
        transform.right = _direction;
        transform.position += _direction.normalized * _speed * Time.deltaTime;
        _currentDistance += Time.deltaTime;
        if (_currentDistance > _maxDistance)
            ReturnBullet();

        RaycastHit2D raycast = Physics2D.Raycast(transform.position, transform.right, .25f, _bulletLayer);

        if (raycast)
        {
            var enemy = raycast.collider.GetComponent<IDamageable>();
            if (enemy != null) enemy.TakeDamage(_dmg);

            ReturnBullet();
        }
    }

    #region BUILDER
    public EnemyBullet SetDirection(Vector2 direction)
    {
        _direction = direction;
        transform.right = _direction;
        return this;
    }
    public EnemyBullet SetPosition(Vector2 position)
    {
        transform.position = position;
        return this;
    }
    public EnemyBullet SetDmg(float dmg)
    {
        _dmg = dmg;
        return this;
    }
    public EnemyBullet SetSpeed(float speed)
    {
        _speed = speed;
        return this;
    }

    public EnemyBullet SetDistance(float distance)
    {
        _maxDistance = distance;
        return this;
    }

    #endregion

    private void Reset()
    {
        _currentDistance = 0;
    }
    public static void TurnOn(EnemyBullet b)
    {
        b.gameObject.SetActive(true);
    }

    public static void TurnOff(EnemyBullet b)
    {
        b.Reset();
        b.gameObject.SetActive(false);
    }
    protected override void ReturnBullet()
    {
        base.ReturnBullet();
        FRY_EnemyBullet.Instance.ReturnBullet(this);
    }
}
