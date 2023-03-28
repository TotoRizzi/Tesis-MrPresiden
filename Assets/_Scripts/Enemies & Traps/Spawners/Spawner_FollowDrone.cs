using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_FollowDrone : Spawner_Enemy
{
    public override IEnumerator SpawnEnemy()
    {
        yield return new WaitForEndOfFrame();
        FRY_Enemy_FollowDrone.Instance.pool.GetObject().SetPosition(transform.position);
    }
}
