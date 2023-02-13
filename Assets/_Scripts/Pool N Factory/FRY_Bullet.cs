using UnityEngine;
public class FRY_Bullet : MonoBehaviour
{
    static FRY_Bullet _instance;
    public static FRY_Bullet Instance { get { return _instance; } }


    [SerializeField] Bullet _prefab;
    [SerializeField] int _stock = 5;

    public ObjectPool<Bullet> pool;


    void Start()
    {
        _instance = this;
        pool = new ObjectPool<Bullet>(BulletCreator, Bullet.TurnOn, Bullet.TurnOff, _stock);
    }

    public Bullet BulletCreator()
    {
        var bullet = Instantiate(_prefab);
        bullet.transform.SetParent(transform);
        return bullet;
    }

    public void ReturnBullet(Bullet b)
    {
        pool.ReturnObject(b);
    }
}
