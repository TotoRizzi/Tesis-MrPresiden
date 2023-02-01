using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController
{
    Player _player;

    public float xAxis;
    public float yAxis;

    public PlayerController(Player player, PlayerView v)
    {
        _player = player;

        _player.OnIdle += v.Idle;
        _player.OnRun += v.Run;
        _player.OnCrouch += v.Crouch;
        _player.OnCrouchIdle += v.CrouchIdle;
        _player.OnCrouchRun += v.CrouchRun;
    }

    public void OnUpdate()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");

        _player.LookAtMouse();
    }


    public void StandingIdleInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _player.fsm.SendInput(Player.PlayerInputs.JUMP);
        else if (xAxis != 0)
            _player.fsm.SendInput(Player.PlayerInputs.STANDINGMOVE);
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            _player.fsm.SendInput(Player.PlayerInputs.CROUCH);
        }
    }

    public void CrouchingIdleInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _player.fsm.SendInput(Player.PlayerInputs.JUMP);
        else if (xAxis != 0)
            _player.fsm.SendInput(Player.PlayerInputs.CROUCHWALKING);
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            _player.fsm.SendInput(Player.PlayerInputs.STAND);
        }
    }

    public void StandingGroundMovingInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _player.fsm.SendInput(Player.PlayerInputs.JUMP);
        else if (xAxis == 0)
            _player.fsm.SendInput(Player.PlayerInputs.STANDINGIDLE);
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            _player.fsm.SendInput(Player.PlayerInputs.CROUCH);
        }
    }

    public void CrouchingGroundMovingInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _player.fsm.SendInput(Player.PlayerInputs.JUMP);
        else if (xAxis == 0)
            _player.fsm.SendInput(Player.PlayerInputs.CROUCHIDLE);
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            _player.fsm.SendInput(Player.PlayerInputs.STAND);
        }
    }

    public void ClimbMovingInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _player.fsm.SendInput(Player.PlayerInputs.JUMP);
    }
}
