using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NewPlayer : MonoBehaviour
{
    #region Components
    public StateMachine fsm;
    PlayerController _controller;
    Rigidbody2D _rb;
    BoxCollider2D _collider;
    WeaponManager _weaponManager;
    #endregion

    #region Movement
    [Header("Movement")]
    [SerializeField] Transform _playerArm;
    [SerializeField] Transform _playerSprite;
    Vector3 _playerDefaultSpriteSize;
    [SerializeField] float _speed;
    #endregion

    #region Jump
    [Header("Jump")]
    GroundCheck _groundCheck;
    [SerializeField] float _jumpForce = 5;
    [SerializeField] int _maxJumps = 1;
    int _currentJumps = 1;
    bool _canJump => _currentJumps > 0;
    float _defaultGravity;
    #endregion

    #region Dash
    [Header("Dash")]
    [SerializeField] float _dashTime;
    [SerializeField] float _dashSpeed;

    float _currentDashTime;
    float _dashDirection;
    #endregion

    #region View
    public event Action OnIdle;
    public event Action OnRun;
    public event Action OnCrouch;
    public event Action OnCrouchIdle;
    public event Action OnCrouchRun;
    #endregion

    private void Start()
    {
        fsm.AddState(StateName.Idle, new IdleState());
        fsm.AddState(StateName.Move, new MoveState());
        fsm.AddState(StateName.Jump, new JumpState());
        fsm.AddState(StateName.OnAir, new OnAirState());
        fsm.AddState(StateName.Dash, new DashState());
        fsm.ChangeState(StateName.Idle);
    }
}

public class IdleState : IState
{
    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
    }
}
public class MoveState : IState
{
    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
    }
}
public class JumpState : IState
{
    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
    }
}
public class OnAirState : IState
{
    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
    }
}
public class DashState : IState
{
    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
    }
}
