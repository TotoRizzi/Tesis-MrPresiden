using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public void EnemyKilled(Vector3 pos)
    {
        FRY_EnemyExplodeParticle.Instance.pool.GetObject().SetPosition(pos);
        FRY_EnemyBloodSplatter.Instance.pool.GetObject().SetPosition(pos);
        FRY_EnemyFeedback.Instance.pool.GetObject().SetPosition(pos).SetText(Helpers.GameManager.EnemyManager.EnemyCountString());
    }

    public void PlayerKilled(Vector3 pos)
    {
        FRY_EnemyExplodeParticle.Instance.pool.GetObject().SetPosition(pos);
        FRY_EnemyBloodSplatter.Instance.pool.GetObject().SetPosition(pos);
    }
}
