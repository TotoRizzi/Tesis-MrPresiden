using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_WaveFollowDrone : Enemy_FollowDrone
{
    [SerializeField] int _coinsDrop;
    Vector3 _randomDir;
    [SerializeField] float _timeToMoveRandom = 1;
    float _currentTimeToMoveRandom;

    public override void ActionOnPlayerDead()
    {
       
    }

    
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

