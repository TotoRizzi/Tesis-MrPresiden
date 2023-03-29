using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_KamikazeRobot : Spawner_Enemy
{
    public override IEnumerator SpawnEnemy()
    {
        yield return new WaitForEndOfFrame();
        FRY_Enemy_KamikazeRobot.Instance.pool.GetObject().SetPosition(transform.position);
    }
}

