using UnityEngine;
public abstract class Spawner_Enemy : MonoBehaviour
{
    private void Start()
    {
        Helpers.GameManager.WaitPlayerDead += SpawnEnemy;
        SpawnEnemy();
    }
    public abstract void SpawnEnemy();
}
