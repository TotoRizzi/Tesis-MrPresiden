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
    public event Action OnUpdate;

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

    bool _canMove = false;

    Vector3 _playerDefaultSpriteSize;
    [SerializeField] Transform _playerSprite;
    public Transform PlayerSprite { get { return _playerSprite; } private set { } }

    float _maxDelayCanMove = .1f;
    float _currentDelayCanMove;
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

    #region Stamina

    [Header("Stamina")]
    [SerializeField] float _staminaPerKill = 10;
    [SerializeField] float _maxStamina = 50;
    public float MaxStamina { get { return _maxStamina; } private set { } }
    float _currentStamina;
    public float CurrentStamina { get { return _currentStamina; } private set { } }

    #endregion

    #region Dash
    [Header("Dash")]
    [SerializeField] float _staminaDashCost;
    [SerializeField] float _waitTimeToReturnStamina = 1f;
    [SerializeField] float _dashSpeed;
    [SerializeField] float _dashDuration;
    private bool _canDash => _currentStamina > 0;
    public bool CanDash { get { return _canDash; } private set { } }
    public float DashDuration { get { return _dashDuration; } private set { } }
    #endregion

    #region View
    public Action OnIdle;
    public Action OnMove;
    public Action OnDash;
    public Action OnJump;
    public Action OnStaminaTick;
    #endregion


    private void Start()
    {
        StartCoroutine(CanMoveDelay());

        //Components
        fsm = new StateMachine();
        _controller = new PlayerController(this, GetComponent<PlayerView>());
        _rb = GetComponent<Rigidbody2D>();
        _groundCheck = GetComponentInChildren<GroundCheck>();
        _weaponManager = GetComponent<WeaponManager>();
        _gameManager = GameManager.instance;

        //Update
        OnUpdate += fsm.Update;
        OnUpdate += _controller.OnUpdate;
        OnUpdate += LookAtMouse;
        OnUpdate += ReturnStamina;
        OnStaminaTick += SaveStamina;
        _gameManager.OnSpiked += FullStamina;

        //Events
        _gameManager.EnemyManager.OnEnemyKilled += GiveStaminaOnKill;

        //Variables
        _maxJumps = 1;
        _currentStamina = _gameManager.SaveDataManager.GetFloat("CurrentStamina", _maxStamina);
        _playerDefaultSpriteSize = _playerSprite.localScale;
        _defaultGravity = _rb.gravityScale;

        //StateMachine
        fsm.AddState(StateName.Idle, new IdleState(this, _controller));
        fsm.AddState(StateName.Move, new MoveState(this, _controller));
        fsm.AddState(StateName.Jump, new JumpState(this, _controller));
        fsm.AddState(StateName.OnAir, new OnAirState(this, _controller));
        fsm.AddState(StateName.Dash, new DashState(this, _controller));
        fsm.AddState(StateName.Climb, new ClimState(this, _controller));
        fsm.ChangeState(StateName.Idle);
    }

    private void Update()
    {
        if(_canMove) OnUpdate?.Invoke();
    }

    private void FixedUpdate()
    {
        if(_canMove) fsm.FixedUpdate();
    }

    IEnumerator CanMoveDelay()
    {
        yield return new WaitForSeconds(_maxDelayCanMove);

        _canMove = true;
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

    void FullStamina()
    {
        _currentStamina = _maxStamina;
        SaveStamina();
    }

    public void ReturnStamina()
    {
        if (_currentStamina >= _maxStamina) return;

        _currentStamina += Time.deltaTime;

        OnStaminaTick?.Invoke();
    }

    public void TakeStamina()
    {
        _currentStamina -= _staminaDashCost;
    }

    void GiveStaminaOnKill()
    {
        _currentStamina += _staminaPerKill;
        ClampStamina();
        OnStaminaTick?.Invoke();
    }

    void SaveStamina()
    {
        _gameManager.SaveDataManager.SaveFloat("CurrentStamina", _currentStamina);
    }

    void ClampStamina()
    {
        _currentStamina = Mathf.Clamp(_currentStamina, 0, _maxStamina);
    }

    public void Dash(float xAxis)
    {
        _rb.velocity = new Vector2(xAxis * _dashSpeed * Time.fixedDeltaTime, 0f);

        ClampStamina();
        OnStaminaTick?.Invoke();
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

    public void FreezeVelocity(bool xAxis = false)
    {
        if (xAxis) _rb.velocity = new Vector2(0, _rb.velocity.y);
        else _rb.velocity = Vector2.zero;
        OnIdle();
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

    public void StopClimging()
    {
        if(fsm.IsInState(StateName.Climb))
            fsm.ChangeState(StateName.OnAir);
    }

    public void ReturnJumps()
    {
        _currentJumps = MaxJumps;
    }

    public IEnumerator GiveStaminaStartTick()
    {
        yield return new WaitForSeconds(_waitTimeToReturnStamina);

        OnUpdate += ReturnStamina;
    }

    public IEnumerator OnAirDelay()
    {
        yield return new WaitForSeconds(.1f);

        if (fsm.IsInState(StateName.Jump))
            fsm.ChangeState(StateName.OnAir);
    }

    public void PausePlayer()
    {
        _canMove = false;
        FreezeVelocity();
    }
    public void UnPausePlayer()
    {
        _canMove = true;
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
        _player.FreezeVelocity(true);
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
        _player.FreezeVelocity();
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
    PlayerController _controller;

    public JumpState(Player player, PlayerController controller)
    {
        _player = player;
        _controller = controller;
    }

    public void OnEnter()
    {
        _player.FreezeVelocity();
        _player.Jump();
        _player.StartCoroutine(_player.OnAirDelay());
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

        _player.OnUpdate -= _player.ReturnStamina;
        _player.TakeStamina();
        _player.StopAllCoroutines();
    }

    public void OnExit()
    {
        _player.FreezeVelocity();

        _player.StartCoroutine(_player.GiveStaminaStartTick());
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