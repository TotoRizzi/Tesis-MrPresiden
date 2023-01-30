using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    Animator _anim;

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
    }

    public void Run()
    {
        _anim.Play("Run");
    }

    public void Idle()
    {
        _anim.Play("Idle");
    }

    public void Crouch()
    {
        _anim.Play("Crouch");
    }
    public void CrouchIdle()
    {
        _anim.Play("CrouchIdle");
    }
    public void CrouchRun()
    {
        _anim.Play("CrouchRun");
    }
}
