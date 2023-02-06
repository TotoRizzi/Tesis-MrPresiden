using UnityEngine;
public class Bullet : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _maxDistance;

    float _currentDistance;
    float _dmg;
    Vector3 _direction;
    private void Update()
    {
        transform.position += _direction.normalized * _speed * Time.deltaTime;
        _currentDistance += Time.deltaTime;
        if (_currentDistance > _maxDistance)
            ReturnBullet();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var entity = collision.GetComponent<IDamageable>();

        if (entity != null) entity.TakeDamage(_dmg);

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

    #endregion

    private void Reset()
    {
        _currentDistance = 0;
    }
    public static void TurnOn(Bullet b)
    {
        b.Reset();
        b.gameObject.SetActive(true);
    }

    public static void TurnOff(Bullet b)
    {
        b.gameObject.SetActive(false);
    }
    void ReturnBullet()
    {
        FRY_Bullet.Instance.ReturnBullet(this);
    }
}
