using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSceneOnTrigger : MonoBehaviour
{
    GameManager _gameManager;

    [SerializeField] GameObject _door;
    Collider2D _collider;
    Animator _anim;

    private void Start()
    {
        _gameManager = GameManager.instance;
        _gameManager.OnRoomWon += ShowDoor;
        _collider = GetComponent<Collider2D>();
        _anim = GetComponent<Animator>();
        HideDoor();
    }
    void ShowDoor()
    {
        _collider.enabled = true;
        _anim.Play("Open");
    }
    void HideDoor()
    {
        _collider.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _gameManager.LevelManager.NextLevel();
    }
}
