using UnityEngine;
public class Breakable : MonoBehaviour, IBreakable, IDamageable
{
    [SerializeField] GameObject _destroyedVersion;

    bool _drop;
    Transform _dropPosition;
    private void Start()
    {
        _drop = Random.value <= .5f;
        if (_drop) _dropPosition = transform.GetChild(0);
    }
    public void Break()
    {
        Instantiate(_destroyedVersion, transform.position, Quaternion.identity);
        if (_drop) FRY_PickUps.Instance.pool.GetObject().SetPosition(_dropPosition.position);
        Destroy(gameObject);
    }

    public void TakeDamage(float dmg)
    {
        Die();
    }

    public void Die()
    {
        Break();
    }
}
