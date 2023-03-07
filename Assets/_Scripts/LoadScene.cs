using UnityEngine;
public class LoadScene : MonoBehaviour
{
    [SerializeField] string _levelName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        PlayerFinalAnimation playerFinal = collision.GetComponent<PlayerFinalAnimation>();
        if (player || playerFinal) GameManager.instance.SceneManager.LoadLevel(_levelName);
    }
}
