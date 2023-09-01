using UnityEngine;
public class FRY_EnemyOilExplodeParticle : MonoBehaviour
{
    static FRY_EnemyOilExplodeParticle _instance;
    public static FRY_EnemyOilExplodeParticle Instance { get { return _instance; } }

    [SerializeField] PS_EnemyExplode _prefab;
    [SerializeField] int _stock = 5;

    public ObjectPool<PS_EnemyExplode> pool;


    void Start()
    {
        _instance = this;
        pool = new ObjectPool<PS_EnemyExplode>(ObjectCreator, PS_EnemyExplode.TurnOn, PS_EnemyExplode.TurnOff, _stock);
    }

    public PS_EnemyExplode ObjectCreator()
    {
        var particle = Instantiate(_prefab);
        particle.transform.SetParent(transform);
        return particle;
    }

    public void ReturnObject(PS_EnemyExplode b)
    {
        pool.ReturnObject(b);
    }
}
