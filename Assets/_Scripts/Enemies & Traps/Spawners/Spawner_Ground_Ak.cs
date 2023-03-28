using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_Ground_Ak : Spawner_Enemy
{
    public override IEnumerator SpawnEnemy()
    {
        yield return new WaitForEndOfFrame();
        FRY_Enemy_Ground_Ak.Instance.pool.GetObject().SetPosition(transform.position);
    }
}
