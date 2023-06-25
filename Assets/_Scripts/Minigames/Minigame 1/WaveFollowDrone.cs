using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFollowDrone : Enemy_FollowDrone
{
    [SerializeField] int _coinsDrop;
    public override void ActionOnPlayerDead()
    {
        
    }
    public override void Die()
    {
        base.Die();
        Helpers.PersistantData.persistantDataSaved.coins += _coinsDrop;
    }
    public override void ReturnObject()
    {
        base.ReturnObject();
        gameManager.EnemyManager.RemoveEnemy(this);

        //GiveMoney
    }
}

