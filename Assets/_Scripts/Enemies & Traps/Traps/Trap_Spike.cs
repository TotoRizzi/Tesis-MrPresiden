using UnityEngine;
public class Trap_Spike : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
            GameManager.instance.PlayerDead();
    }
}
