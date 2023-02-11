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

    public void StaminaTick()
    {
        _gameManager.UiManager.UpdateStaminaBar(_player.CurrentStamina, _player.MaxStamina);
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

