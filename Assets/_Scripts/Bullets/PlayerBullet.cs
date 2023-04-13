using UnityEngine;
public class PlayerBullet : Bullet
{
    Vector3 lastPosition;
    private void FixedUpdate()
    {
        transform.position += _direction.normalized * _speed * Time.fixedDeltaTime;
        _currentDistance += Time.fixedDeltaTime;
        if (_currentDistance > _maxDistance)
            ReturnBullet();

        Vector3 direction = transform.position - lastPosition;
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, lastPosition, direction.magnitude, _bulletLayer);

        if (raycast)
        {
            var enemy = raycast.collider.GetComponent<IDamageable>();
            if (enemy != null) enemy.TakeDamage(_dmg);
            else Helpers.AudioManager.PlaySFX("Bullet_GroundHit");

            ReturnBullet();
        }

        lastPosition = transform.position;
    }

    #region BUILDER
    public PlayerBullet SetDirection(Vector2 direction)
    {
        _direction = direction;
        transform.right = _direction;
        return this;
    }
    public PlayerBullet SetPosition(Vector2 position)
    {
        transform.position = position;
        return this;
    }
    public PlayerBullet SetDmg(float dmg)
    {
        _dmg = dmg;
        return this;
    }
    public PlayerBullet SetSpeed(float speed)
    {
        _speed = speed;
        return this;
    }

    public PlayerBullet SetDistance(float distance)
    {
        _maxDistance = distance;
        return this;
    }

    #endregion

    private void Reset()
    {
        _currentDistance = 0;
    }
    public static void TurnOn(PlayerBullet b)
    {
        b.gameObject.SetActive(true);
        b.lastPosition = Vector3.zero;
    }

    public static void TurnOff(PlayerBullet b)
    {
        b.Reset();
        b.gameObject.SetActive(false);
    }
    protected override void ReturnBullet()
    {
        FRY_PlayerBullet.Instance.ReturnBullet(this);
    }
}
