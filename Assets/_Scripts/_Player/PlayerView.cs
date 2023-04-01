using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    GameManager _gameManager;

    [SerializeField] Animator _anim;
    Player _player;

    private void Start()
    {
        _gameManager = GameManager.instance;
        _player = _gameManager.Player;
    }

    public void Run()
    {
        _anim.Play("Run");
    }

    public void Idle()
    {
        _anim.Play("Idle");
    }

    public void Jump()
    {
        Helpers.AudioManager.PlaySFX("Player_Jump");
    }

    public void Dash()
    {
        Helpers.AudioManager.PlaySFX("Player_Dash");
    }
}

