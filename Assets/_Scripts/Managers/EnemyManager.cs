using UnityEngine;
using System;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public Action OnEnemyKilled;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void EnemyKilled()
    {
        OnEnemyKilled();
    }
}
