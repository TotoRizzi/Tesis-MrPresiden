using UnityEngine;
public class NextSceneOnTrigger : MonoBehaviour
{
    [SerializeField] bool _isTutorial;
    [SerializeField] int _nextScene;
    [SerializeField] GameObject lightParent;

    Collider2D _collider;
    Animator _anim;
    private void Start()
    {
        Helpers.GameManager.OnRoomWon += ShowDoor;
        _collider = GetComponent<Collider2D>();
        _anim = GetComponentInChildren<Animator>();
        HideDoor();
    }
    void ShowDoor()
    {
        _collider.enabled = true;
        lightParent.SetActive(true);
        _anim.Play("Open");
    }
    void HideDoor()
    {
        lightParent.SetActive(false);
        _collider.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isTutorial) return;
        if (Helpers.GameManager.UiManager == null) return;

        Helpers.GameManager.LoadSceneManager.LoadLevel(_nextScene);
        Helpers.GameManager.UiManager.CloseCurtain();
        if(Helpers.GameManager.Player) Helpers.GameManager.Player.PausePlayer();
    }
}
