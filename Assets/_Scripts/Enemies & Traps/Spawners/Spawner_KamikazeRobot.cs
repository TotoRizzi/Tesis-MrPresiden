public class Spawner_KamikazeRobot : Spawner_Enemy
{
    public override void SpawnEnemy()
    {
        FRY_Enemy_KamikazeRobot.Instance.pool.GetObject().SetPosition(transform.position);
    }
}

