using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Spike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.instance.PlayerDead();
    }
}
