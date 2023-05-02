using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_KamikazeRobot : Spawner_Enemy
{
    [SerializeField] bool _static;

    public override IEnumerator SpawnEnemy()
    {
        yield return new WaitForEndOfFrame();
        Enemy_KamikazeRobot enemy = (Enemy_KamikazeRobot)FRY_Enemy_KamikazeRobot.Instance.pool.GetObject().SetPosition(transform.position);

        enemy.IsStatic(_static);
    }
}

