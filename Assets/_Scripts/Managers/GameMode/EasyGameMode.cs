using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyGameMode : GameModeManager
{
    public override void PlayerDead()
    {
        playerHealth.EffectsOnDeath();
        playerHealth.RestartPosition();

        Debug.Log("EasyGameMode");
    }
}
