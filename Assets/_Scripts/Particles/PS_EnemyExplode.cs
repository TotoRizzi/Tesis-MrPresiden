using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS_EnemyExplode : Particle
{
    public override void ReturnObject()
    {
        FRY_EnemyExplodeParticle.Instance.ReturnObject(this);
    }
}
