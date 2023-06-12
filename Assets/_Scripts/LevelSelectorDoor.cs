using UnityEngine;
public class LevelSelectorDoor : MonoBehaviour
{
    [SerializeField] int _levelIndex;

    Animator _anim;
    Collider2D _collider;
    private void Start()
    {
        if (_levelIndex > Helpers.PersistantData.persistantDataSaved.currentLevel) return;
        _anim = GetComponentInChildren<Animator>();
        _collider = GetComponent<Collider2D>();

        ShowExit();
    }

    void ShowExit()
    {
        _anim.SetBool("IsOpen", true);
        _collider.isTrigger = true;
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
