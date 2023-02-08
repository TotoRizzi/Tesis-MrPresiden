using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FRY_FollowDrone : MonoBehaviour
{
    static FRY_FollowDrone _instance;
    public static FRY_FollowDrone Instance { get { return _instance; } }

    [SerializeField] Enemy_FollowDrone _prefab;
    [SerializeField] int _stock = 5;

    public ObjectPool<Enemy_FollowDrone> pool;
    void Start()
    {
        _instance = this;
        pool = new ObjectPool<Enemy_FollowDrone>(ObjectCreator, Enemy_FollowDrone.TurnOn, Enemy_FollowDrone.TurnOff, _stock);
    }

    public Enemy_FollowDrone ObjectCreator()
    {
        return Instantiate(_prefab);
    }

    public void ReturnObject(Enemy_FollowDrone b)
    {
        pool.ReturnObject(b);
    }
}
