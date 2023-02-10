using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    GameManager _gameManager;
    [SerializeField] Animator _anim;

    private void Start()
    {
        _gameManager = GameManager.instance;
    }

    public void Run()
    {
        _anim.Play("Run");
        Debug.Log("Running");
    }

    public void Idle()
    {
        _anim.Play("Idle");
    }

    public void Jump()
    {
        _gameManager.SoundManager.PlaySound("Player_Jump");
    }

    public void Dash()
    {
        _gameManager.SoundManager.PlaySound("Player_Dash");
    }
}

