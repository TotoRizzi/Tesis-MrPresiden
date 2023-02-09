using UnityEngine;
public class Breakable : MonoBehaviour, IBreakable
{
    [SerializeField] GameObject _destroyedVersion;
    [SerializeField] bool _drop;

    Transform _dropPosition;
    private void Start()
    {
        if (_drop) _dropPosition = transform.GetChild(0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var bullet = collision.GetComponent<Bullet>();
        if (bullet)
            Break();
    }
    public void Break()
    {
        Instantiate(_destroyedVersion, transform.position, Quaternion.identity);
        if (_drop) FRY_PickUps.Instance.pool.GetObject().SetPosition(_dropPosition.position);
        Destroy(gameObject);
    }
}
