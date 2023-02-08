using UnityEngine;
public class Breakable : MonoBehaviour, IBreakable
{
    [SerializeField] GameObject _destroyedVersion;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var bullet = collision.GetComponent<Bullet>();
        if (bullet)
        {
            Break();
            Debug.Log("Bullet");
        }
    }
    public void Break()
    {
        Instantiate(_destroyedVersion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
