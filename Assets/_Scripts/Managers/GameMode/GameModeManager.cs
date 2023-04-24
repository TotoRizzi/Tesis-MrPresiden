using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    protected GameManager gameManager;
    protected PlayerHealth playerHealth;

    private void Start()
    {
        gameManager = Helpers.GameManager;
        playerHealth = gameManager.Player.GetComponent<PlayerHealth>();

        gameManager.OnPlayerDead += PlayerDead;
    }

    public virtual void PlayerDead()
    {

    }
}
