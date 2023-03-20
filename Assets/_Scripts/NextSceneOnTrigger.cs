using UnityEngine;
public class NextSceneOnTrigger : MonoBehaviour
{
    GameManager _gameManager;

    [SerializeField] bool _isTutorial;
    [SerializeField] GameObject _door;
    [SerializeField] GameObject lightParent;
    Collider2D _collider;
    Animator _anim;
    private void Start()
    {
        _gameManager = GameManager.instance;
        _gameManager.OnRoomWon += ShowDoor;
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
        _gameManager.LevelManager.NextLevel();
    }
}
