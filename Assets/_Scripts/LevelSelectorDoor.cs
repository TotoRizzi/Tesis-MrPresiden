using UnityEngine;
using System.Linq;
public class LevelSelectorDoor : MonoBehaviour
{
    [SerializeField] int _levelIndex;

    Animator _anim;
    Collider2D _colliderNoTrigger;
    private void Start()
    {
        if (_levelIndex > Helpers.PersistantData.persistantDataSaved.currentLevel) return;
        _anim = GetComponentInChildren<Animator>();
        _colliderNoTrigger = GetComponents<Collider2D>().Where(x => !x.isTrigger).FirstOrDefault();

        ShowExit();
    }

    void ShowExit()
    {
        _anim.SetBool("IsOpen", true);
        _colliderNoTrigger.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            Helpers.GameManager.LoadSceneManager.LoadLevel("Level " + _levelIndex);
            Helpers.GameManager.Player.PausePlayer();
        }
    }
}
