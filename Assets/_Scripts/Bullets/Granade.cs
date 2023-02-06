using UnityEngine;
using System.Linq;
public class Granade : MonoBehaviour
{
    [SerializeField] float _throwForce;
    [SerializeField] float _explosionRadius;
    [SerializeField] float _explosionForce;

    Rigidbody2D _rb;
    Vector2 _direction;

    float _triggerForce = .5f;
    float _damage;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var entity = collision.gameObject.GetComponent<IDamageable>();

        if (entity != null) entity.TakeDamage(_damage);

        if (collision.relativeVelocity.magnitude >= _triggerForce) Explosion();
    }

    public void ThrowGranade()
    {
        _rb.AddForce(_direction * _throwForce, ForceMode2D.Impulse);
    }
    void Explosion()
    {
        var collisions = Physics2D.CircleCastAll(transform.position, _explosionRadius, Vector2.one, GameManager.instance.DynamicBodiesLayer).
                                                                                                                                            Where(x => x.collider.GetComponent<Rigidbody2D>() != null).
                                                                                                                                            Select(x => x.collider.GetComponent<Rigidbody2D>()).Where(x => !x.GetComponent<Player>());
        if (collisions.Count() <= 0) return;

        foreach (var item in collisions)
            item.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);

        Destroy(gameObject);
    }

    #region BUILDER 

    public Granade SetPosition(Vector2 position)
    {
        transform.position = position;
        return this;
    }
    public Granade SetDirection(Vector2 direction)
    {
        _direction = direction;
        return this;
    }

    public Granade SetLayer(Layers layer)
    {
        gameObject.layer = (int)layer;
        return this;
    }

    public Granade SetDamage(float damage)
    {
        _damage = damage;
        return this;
    }

    #endregion

    private void Reset()
    {
        _rb.velocity = Vector2.zero;
    }
    public static void TurnOn(Granade g)
    {
        g.gameObject.SetActive(true);
        g.Reset();
    }
    public static void TurnOff(Granade g)
    {
        g.gameObject.SetActive(false);
    }
}
