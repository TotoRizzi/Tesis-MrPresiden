using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    GameManager _gameManager;

    [SerializeField] Animator _anim;
    [SerializeField] ParticleSystem _dashParticle;

    private void Start()
    {
        _gameManager = GameManager.instance;
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
        _dashParticle.Play();
        Helpers.AudioManager.PlaySFX("Player_Dash");
    }
}

