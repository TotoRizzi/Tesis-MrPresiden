using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_Sniper : Spawner_Enemy
{
    public override IEnumerator SpawnEnemy()
    {
        yield return new WaitForEndOfFrame();
        FRY_Enemy_Sniper.Instance.pool.GetObject().SetPosition(transform.position);
    }
}
