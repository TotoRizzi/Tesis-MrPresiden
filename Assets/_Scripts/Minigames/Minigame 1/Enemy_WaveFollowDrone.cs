using UnityEngine;
public class Enemy_WaveFollowDrone : Enemy_FollowDrone
{
    [SerializeField] int _coinsDrop;
    public override void Die()
    {
        base.Die();
        Helpers.PersistantData.persistantDataSaved.coins += _coinsDrop;
        FRY_CoinFeedback.Instance.pool.GetObject().SetPosition(transform.position);
    }

    public override void ReturnObject()
    {
        base.ReturnObject();
        gameManager.EnemyManager.RemoveEnemy(this);
    }
}

