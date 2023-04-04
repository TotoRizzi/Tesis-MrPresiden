using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController
{
    Player _player;

    public float xAxis { get; private set; }
    public float yAxis { get; private set; }

    public PlayerController(Player player, PlayerView v)
    {
        _player = player;

        _player.OnIdle += v.Idle;
        _player.OnMove += v.Run;
        _player.OnJump += v.Jump;
        _player.OnDash += v.Dash;
    }

    public void OnUpdate()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
    }

    public void IdleInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _player.CanJump)
            _player.fsm.ChangeState(StateName.Jump);
        else if (xAxis != 0)
            _player.fsm.ChangeState(StateName.Move);
        if (Input.GetKeyDown(KeyCode.LeftShift) && _player.CanDash)
            _player.fsm.ChangeState(StateName.Dash);
    }

    public void MovingInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _player.CanJump)
            _player.fsm.ChangeState(StateName.Jump);
        else if (xAxis == 0)
            _player.fsm.ChangeState(StateName.Idle);
        if (Input.GetKeyDown(KeyCode.LeftShift) && _player.CanDash)
            _player.fsm.ChangeState(StateName.Dash);
    }

    public void OnAirInputs()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _player.CanDash)
            _player.fsm.ChangeState(StateName.Dash);
        else if (Input.GetKeyDown(KeyCode.Space) && _player.CanJump)
            _player.fsm.ChangeState(StateName.Jump);
    }

    public void OnJumpInputs()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _player.CanDash)
            _player.fsm.ChangeState(StateName.Dash);
    }

    public void ClimbingInputs()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _player.CanDash)
            _player.fsm.ChangeState(StateName.Dash);
        else if (Input.GetKeyDown(KeyCode.Space) && _player.CanJump)
            _player.fsm.ChangeState(StateName.Jump);
    }
}
