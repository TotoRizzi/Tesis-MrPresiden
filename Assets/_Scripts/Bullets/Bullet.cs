using UnityEngine;
public class Bullet : MonoBehaviour
{
    [SerializeField] float _speed;

    float _maxDistance = 5;
    float _currentDistance;
    float _dmg;
    Vector3 _direction;

    private void Start()
    {
        Helpers.GameManager.OnPlayerDead += ReturnBullet;
    }

    private void FixedUpdate()
    {
        transform.position += _direction.normalized * _speed * Time.fixedDeltaTime;
        _currentDistance += Time.fixedDeltaTime;
        if (_currentDistance > _maxDistance)
            ReturnBullet();

        RaycastHit2D dmg = Physics2D.Raycast(transform.position, _direction.normalized, .4f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var entity = collision.GetComponent<IDamageable>();

        if (entity != null) entity.TakeDamage(_dmg);
        else Helpers.AudioManager.PlaySFX("Bullet_GroundHit");

        ReturnBullet();
    }

    #region BUILDER
    public Bullet SetDirection(Vector2 direction)
    {
        _direction = direction;
        transform.right = _direction;
        return this;
    }
    public Bullet SetPosition(Vector2 position)
    {
        transform.position = position;
        return this;
    }
    public Bullet SetDmg(float dmg)
    {
        _dmg = dmg;
        return this;
    }
    public Bullet SetLayer(Layers layer)
    {
        gameObject.layer = (int)layer;
        return this;
    }
    public Bullet SetSpeed(float speed)
    {
        _speed = speed;
        return this;
    }

    public Bullet SetDistance(float distance)
    {
        _maxDistance = distance;
        return this;
    }

    #endregion

    private void Reset()
    {
        _currentDistance = 0;
    }
    public static void TurnOn(Bullet b)
    {
        b.gameObject.SetActive(true);
    }

    public static void TurnOff(Bullet b)
    {
        b.Reset();
        b.gameObject.SetActive(false);
    }
    void ReturnBullet()
    {
        FRY_Bullet.Instance.ReturnBullet(this);
    }
}
