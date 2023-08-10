using UnityEngine;
public class Spawner_ShootingRobot : Spawner_Enemy
{
    [SerializeField] bool _flip;

    public override void SpawnEnemy()
    {
        if(_flip) FRY_Enemy_ShootingRobot.Instance.pool.GetObject().SetPosition(transform.position).Flip();
        else FRY_Enemy_ShootingRobot.Instance.pool.GetObject().SetPosition(transform.position);
    }
}
