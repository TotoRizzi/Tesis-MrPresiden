using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner_Enemy : MonoBehaviour
{
    private void Start()
    {
        Helpers.GameManager.OnSpiked += () => StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnEnemy());
    }
    public abstract IEnumerator SpawnEnemy();
}
