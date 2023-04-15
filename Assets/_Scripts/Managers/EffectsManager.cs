using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public void HumanoindKilled(Vector3 pos)
    {
        FRY_EnemyExplodeParticle.Instance.pool.GetObject().SetPosition(pos);
        FRY_EnemyBloodSplatter.Instance.pool.GetObject().SetPosition(pos);
        FRY_ClockFeedback.Instance.pool.GetObject().SetPosition(pos);
    }
}
