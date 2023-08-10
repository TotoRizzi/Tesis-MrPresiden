public class Spawner_Sniper : Spawner_Enemy
{
    public override void SpawnEnemy()
    {
        FRY_Enemy_Sniper.Instance.pool.GetObject().SetPosition(transform.position);
    }
}
