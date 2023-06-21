using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFollowDrone : Enemy_FollowDrone
{
    public override void ActionOnPlayerDead()
    {
        
    }
    public override void ReturnObject()
    {
        base.ReturnObject();
        gameManager.EnemyManager.RemoveEnemy(this);

        //GiveMoney
    }
}

