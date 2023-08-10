public class Spawner_Ground_Ak : Spawner_Enemy
{
    public override void SpawnEnemy()
    {
        FRY_Enemy_Ground_Ak.Instance.pool.GetObject().SetPosition(transform.position);
    }
}
