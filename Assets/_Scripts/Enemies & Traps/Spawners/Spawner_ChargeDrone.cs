using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_ChargeDrone : Spawner_Enemy
{
    public override IEnumerator SpawnEnemy()
    {
        yield return new WaitForEndOfFrame();
        FRY_Enemy_ChargeDrone.Instance.pool.GetObject().SetPosition(transform.position);
    }
}
