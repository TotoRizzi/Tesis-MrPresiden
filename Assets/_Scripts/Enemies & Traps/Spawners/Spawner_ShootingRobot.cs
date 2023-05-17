using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_ShootingRobot : Spawner_Enemy
{
    [SerializeField] bool _flip;

    public override IEnumerator SpawnEnemy()
    {
        yield return new WaitForEndOfFrame();

        if(_flip) FRY_Enemy_ShootingRobot.Instance.pool.GetObject().SetPosition(transform.position).Flip();
        else FRY_Enemy_ShootingRobot.Instance.pool.GetObject().SetPosition(transform.position);
    }
}
