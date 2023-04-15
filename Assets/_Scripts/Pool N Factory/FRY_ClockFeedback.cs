using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FRY_ClockFeedback : MonoBehaviour
{
    static FRY_ClockFeedback _instance;
    public static FRY_ClockFeedback Instance { get { return _instance; } }


    [SerializeField] ClockFeedback _prefab;
    [SerializeField] int _stock = 5;

    public ObjectPool<ClockFeedback> pool;


    void Start()
    {
        _instance = this;
        pool = new ObjectPool<ClockFeedback>(ObjectCreator, ClockFeedback.TurnOn, ClockFeedback.TurnOff, _stock);
    }

    public ClockFeedback ObjectCreator()
    {
        var enemy = Instantiate(_prefab);
        enemy.transform.SetParent(transform);
        return enemy;
    }

    public void ReturnObject(ClockFeedback b)
    {
        pool.ReturnObject(b);
    }
}
