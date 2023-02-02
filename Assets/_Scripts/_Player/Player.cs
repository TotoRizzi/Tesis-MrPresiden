using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IA2;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerView))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    #region Components
    [HideInInspector] public StateMachine fsm;

    PlayerController _controller;
    Rigidbody2D _rb;

    WeaponManager _weaponManager;
    public WeaponManager WeaponManager { get { return _weaponManager; } private set { } }

    GroundCheck _groundCheck;
    public GroundCheck GroundCheck { get { return _groundCheck; } private set { } }
    #endregion

    #region Movement
    [Header("Movement")]
    [SerializeField] float _speed;

    Vector3 _playerDefaultSpriteSize;
    [SerializeField] Transform _playerSprite;
    public Transform PlayerSprite { get { return _playerSprite; } private set { } }
    #endregion

    #region Jump
    [Header("Jump")]
    [SerializeField] float _jumpForce = 5;

    [SerializeField] int _maxJumps = 1;
    public int MaxJumps { get { return _maxJumps; } private set { } }

    [HideInInspector] public int _currentJumps = 1;
  
    bool _canJump => _currentJumps > 0;
    #endregion

    #region Dash
    [Header("Dash")]
    [SerializeField] float _dashSpeed;
    [SerializeField] float _dashDuration;
    public float DashDuration { get { return _dashDuration; } private set { } }
    #endregion

    #region View
    public Action OnIdle;
    public Action OnMove;
    #endregion


    private void Start()
    {
        fsm = new StateMachine();
        _controller = new PlayerController(this, GetComponent<PlayerView>());
        _rb = GetComponent<Rigidbody2D>();
        _groundCheck = GetComponentInChildren<GroundCheck>();
        _weaponManager = GetComponent<WeaponManager>();

        _playerDefaultSpriteSize = _playerSprite.localScale;

        fsm.AddState(StateName.Idle, new IdleState(this, _controller));
        fsm.AddState(StateName.Move, new MoveState(this, _controller));
        fsm.AddState(StateName.Jump, new JumpState(this));
        fsm.AddState(StateName.OnAir, new OnAirState(this, _controller));
        fsm.AddState(StateName.Dash, new DashState(this, _controller));
        fsm.ChangeState(StateName.Idle);
    }

    private void Update()
    {
        fsm.Update();
        _controller.OnUpdate();

        LookAtMouse();
    }

    private void FixedUpdate()
    {
        fsm.FixedUpdate();
    }

    public void Move(float axis)
    {
        _rb.velocity = new Vector2(axis * _speed * Time.fixedDeltaTime, _rb.velocity.y);
    }

    public void Jump()
    {
        if (!_canJump) return;

        FreezeVelocity();

        _currentJumps--;
        _rb.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);
    }

    public void Dash(float xAxis)
    {
        _rb.velocity = new Vector2(xAxis * _dashSpeed * Time.fixedDeltaTime, _rb.velocity.y);
    }

    public void LookAtMouse()
    {
        Vector3 playerLocalScale = _playerDefaultSpriteSize;
        float angle = _weaponManager.GetAngle();

        if (angle > 90 || angle < -90)
            playerLocalScale.x = -_playerDefaultSpriteSize.x;
        else
            playerLocalScale.x = _playerDefaultSpriteSize.x;

        PlayerSprite.localScale = playerLocalScale;
    }

    public void AddExtraJump()
    {
        _maxJumps++;
    }

    public void FreezeVelocity()
    {
        _rb.velocity = Vector2.zero;
    }
}


public class IdleState : IState
{
    Player _player;
    PlayerController _controller;

    public IdleState(Player player, PlayerController controller)
    {
        _player = player;
        _controller = controller;
    }

    public void OnEnter()
    {
        _player.FreezeVelocity();
        _player.OnIdle();
    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {

    }

    public void OnUpdate()
    {
        _controller.IdleInputs();
    }
}
public class MoveState : IState
{
    Player _player;
    PlayerController _controller;

    public MoveState(Player player, PlayerController controller)
    {
        _player = player;
        _controller = controller;
    }

    public void OnEnter()
    {
        _player.OnMove();
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
        _player.Move(_controller.xAxis);
    }

    public void OnUpdate()
    {
        _controller.MovingInputs();
    }
}
public class JumpState : IState
{
    Player _player;

    public JumpState(Player player)
    {
        _player = player;
    }

    public void OnEnter()
    {
        _player.Jump();
        _player.fsm.ChangeState(StateName.OnAir);
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
        Debug.Log("Jump");

    }
}
public class OnAirState : IState
{
    Player _player;
    PlayerController _controller;

    public OnAirState(Player player, PlayerController controller)
    {
        _player = player;
        _controller = controller;
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
        _player._currentJumps = _player.MaxJumps;
    }

    public void OnFixedUpdate()
    {
        _player.Move(_controller.xAxis);
    }

    public void OnUpdate()
    {
        _controller.OnAirInputs();
        if (_player.GroundCheck.IsGrounded) _player.fsm.ChangeState(StateName.Idle);
    }
}
public class DashState : IState
{
    Player _player;
    PlayerController _controller;

    float _currentDashDuration;
    float _dashDirection;

    public DashState(Player player, PlayerController controller)
    {
        _player = player;
        _controller = controller;
    }

    public void OnEnter()
    {
        _currentDashDuration = 0;
       
        if(_controller.xAxis != 0)
        {
            _dashDirection = _controller.xAxis;
        }
        else
        {
            float angle = _player.WeaponManager.GetAngle();
        
            if (angle > 90 || angle < -90)
                _dashDirection = -1;
            else
                _dashDirection = 1;
        }
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
        _player.Dash(_dashDirection);
    }

    public void OnUpdate()
    {
        _currentDashDuration += Time.deltaTime;
        if (_currentDashDuration < _player.DashDuration) return;

        if (_player.GroundCheck.IsGrounded) _player.fsm.ChangeState(StateName.OnAir);
        else _player.fsm.ChangeState(StateName.Idle);
    }
}