using UnityEngine;
using System.Linq;
public class Granade : MonoBehaviour
{
    [SerializeField] float _throwForce;
    [SerializeField] float _explosionRadius;
    [SerializeField] float _explosionForce;

    public float ThrowForce { get { return _throwForce; } }

    Rigidbody2D _rb;
    Vector2 _direction;
    float _damage;
    LayerMask _groundLayer;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        _groundLayer = GameManager.instance.GroundLayer;
    }
    private void Update()
    {
        transform.right = _rb.velocity;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            Explosion();
            FRY_Granades.Instance.ReturnBullet(this);
        }
    }

    public void ThrowGranade()
    {
        _rb.AddForce(_direction * _throwForce, ForceMode2D.Impulse);
    }
    void Explosion()
    {
        var collisions = Physics2D.CircleCastAll(transform.position, _explosionRadius, Vector2.one, GameManager.instance.DynamicBodiesLayer).
                                                                                                                                            Where(x => x.collider.GetComponent<IDamageable>() != null && x.collider.GetComponent<Rigidbody2D>() != null && !x.collider.GetComponent<Player>()).
                                                                                                                                            Select(x => x.collider.GetComponent<Rigidbody2D>());
        if (collisions.Count() <= 0) return;

        foreach (var item in collisions)
        {
            if (!InSight(item.position, _groundLayer)) return;
            item.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
            var breakable = item.GetComponent<IDamageable>();
            if (breakable != null) breakable.TakeDamage(_damage);
        }
    }

    bool InSight(Vector3 other, LayerMask ground)
    {
        return !Physics2D.Raycast(transform.position, other - transform.position, (other - transform.position).magnitude, ground);
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

    #region FACTORY
    private void Reset()
    {
        _rb.velocity = Vector2.zero;
    }
    public static void TurnOn(Granade g)
    {
        g.gameObject.SetActive(true);
    }
    public static void TurnOff(Granade g)
    {
        g.Reset();
        g.gameObject.SetActive(false);
    }

    #endregion
}
