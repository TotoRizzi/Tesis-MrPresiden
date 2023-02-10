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

    GameManager _gameManager;
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
    float _defaultGravity;

    [SerializeField] int _maxJumps = 1;
    public int MaxJumps { get { return _maxJumps; } private set { } }

    public int _currentJumps = 1;
  
    bool _canJump => _currentJumps > 0;
    #endregion

    #region Dash
    [Header("Dash")]
    [SerializeField] float _dashSpeed;
    [SerializeField] float _dashDuration;
    public bool canDash { get; private set; }
    public float DashDuration { get { return _dashDuration; } private set { } }
    #endregion

    #region View
    public Action OnIdle;
    public Action OnMove;
    public Action OnDash;
    public Action OnJump;
    #endregion


    private void Start()
    {
        //Components
        fsm = new StateMachine();
        _controller = new PlayerController(this, GetComponent<PlayerView>());
        _rb = GetComponent<Rigidbody2D>();
        _groundCheck = GetComponentInChildren<GroundCheck>();
        _weaponManager = GetComponent<WeaponManager>();
        _gameManager = GameManager.instance;

        //Variables
        canDash = true;
        _maxJumps = _gameManager.SaveDataManager.GetInt("MaxJumps", 1);
        _playerDefaultSpriteSize = _playerSprite.localScale;
        _defaultGravity = _rb.gravityScale;

        //StateMachine
        fsm.AddState(StateName.Idle, new IdleState(this, _controller));
        fsm.AddState(StateName.Move, new MoveState(this, _controller));
        fsm.AddState(StateName.Jump, new JumpState(this));
        fsm.AddState(StateName.OnAir, new OnAirState(this, _controller));
        fsm.AddState(StateName.Dash, new DashState(this, _controller));
        fsm.AddState(StateName.Climb, new ClimState(this, _controller));
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

    public void ClimbMove(float axis)
    {
        _rb.velocity = new Vector2(_rb.velocity.x, axis * _speed * Time.fixedDeltaTime);
    }

    public void Jump()
    {
        if (!_canJump) return;

        FreezeVelocity();

        OnJump();
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode2D.Impulse);
        _currentJumps--;
    }

    public void Dash(float xAxis)
    {
        _rb.velocity = new Vector2(xAxis * _dashSpeed * Time.fixedDeltaTime, 0f);
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
        _gameManager.SaveDataManager.SaveInt("MaxJumps", _maxJumps);
    }

    public void FreezeVelocity()
    {
        _rb.velocity = Vector2.zero;
    }

    public void CeroGravity()
    {
        _rb.gravityScale = 0;
    }

    public void NormalGravity()
    {
        _rb.gravityScale = _defaultGravity;
    }

    public void Climb()
    {
        fsm.ChangeState(StateName.Climb);
    }

    public void ReturnJumps()
    {
        _currentJumps = MaxJumps;
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
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
        _player.fsm.ChangeState(StateName.OnAir);
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
        
    }

    public void OnFixedUpdate()
    {
        _player.Move(_controller.xAxis);
    }

    public void OnUpdate()
    {
        _controller.OnAirInputs();
        if (_player.GroundCheck.IsGrounded)
        {
            _player.ReturnJumps();
            _player.fsm.ChangeState(StateName.Idle);
        }
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
        _player.OnDash();

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
public class ClimState : IState
{
    Player _player;
    PlayerController _controller;

    public ClimState(Player player, PlayerController controller)
    {
        _player = player;
        _controller = controller;
    }

    public void OnEnter()
    {
        _player.FreezeVelocity();
        _player.CeroGravity();
        _player.ReturnJumps();
    }

    public void OnExit()
    {
        _player.NormalGravity();
    }

    public void OnFixedUpdate()
    {
        _player.ClimbMove(_controller.yAxis);
    }

    public void OnUpdate()
    {
        _controller.ClimbingInputs();
    }
}