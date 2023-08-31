using System.Collections;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerView))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : GeneralPlayer
{
    public event Action OnUpdate;
    public Action ExitClimb;

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

    [SerializeField] float _maxDelayCanMove = .2f;
    #endregion

    #region Jump
    [Header("Jump")]
    [SerializeField] float _jumpForce = 5;
    float _defaultGravity;
    bool _canJump => !_alreadyJumped;

    bool _alreadyJumped;
    public bool CanJump { get { return _canJump; } private set { } }
    public bool AlreadyJumped { get { return _alreadyJumped; } set { _alreadyJumped = value; } }
    #endregion

    #region Dash
    [Header("Dash")]
    [SerializeField] float _dashSpeed;
    [SerializeField] float _dashDuration;
    float _dashCooldown = 3f;
    float _dashTimer;
    public float DashDuration { get { return _dashDuration; } private set { } }

    bool _canDash => _dashTimer <= 0;
    public bool CanDash { get { return _canDash; } private set { } }

    #endregion

    #region View
    public Action OnIdle;
    public Action OnMove;
    public Action<Vector3> OnDash;
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
        _gameManager = Helpers.GameManager;

        //Update
        OnUpdate += fsm.Update;
        OnUpdate += _controller.OnUpdate;
        OnUpdate += LookAtMouse;

        //Variables
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

        _gameManager.OnPlayerDead += () => fsm.ChangeState(StateName.Idle);
        _gameManager.OnPlayerDead += ReturnJumps;
    }

    private void Update()
    {
        _dashTimer -= Time.deltaTime;
        if (_canMove) OnUpdate?.Invoke();
    }

    private void FixedUpdate()
    {
        if (_canMove) fsm.FixedUpdate();
    }
    public IEnumerator CanMoveDelay()
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
        FreezeVelocity();

        OnJump();
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode2D.Impulse);
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
        if (fsm.IsInState(StateName.Climb))
            fsm.ChangeState(StateName.OnAir);
    }

    public void ReturnJumps()
    {
        _dashTimer = 0;
    }

    public void BlockDash()
    {
        _dashTimer = _dashCooldown;
    }

    public override void PausePlayer()
    {
        _canMove = false;
        CeroGravity();
        FreezeVelocity();
    }
    public override void UnPausePlayer()
    {
        StartCoroutine(CanMoveDelay());
        NormalGravity();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Rope")
            EnterRope(collision.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Rope")
            ExitRope();
    }

    void EnterRope(GameObject rope)
    {
        var _rope = rope;

        transform.position = new Vector2(_rope.transform.position.x, transform.position.y);
        Climb();
    }
    void ExitRope()
    {
        StopClimging();
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
        _player.AlreadyJumped = false;
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
    string[] _stepsSounds = new string[2] { "Footstep1", "Footstep2" };

    float _timer;
    int _index;
    public MoveState(Player player, PlayerController controller)
    {
        _player = player;
        _controller = controller;
    }

    public void OnEnter()
    {
        Helpers.LevelTimerManager.StartLevelTimer();
        _player.OnMove();
        _player.AlreadyJumped = false;
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
        _timer += Time.deltaTime;
        if (_timer >= .1f)
        {
            Helpers.AudioManager.PlaySFX(_stepsSounds[_index++ % _stepsSounds.Length]);
            _timer = 0;
        }
    }
}
public class JumpState : IState
{
    Player _player;
    PlayerController _controller;

    float _onAirTimer = .1f;
    float _currentOnAirTimer;

    public JumpState(Player player, PlayerController controller)
    {
        _player = player;
        _controller = controller;
    }

    public void OnEnter()
    {
        Helpers.LevelTimerManager.StartLevelTimer();

        //_player.FreezeVelocity();
        _currentOnAirTimer = 0;
        _player.Jump();
        _player.AlreadyJumped = true;
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
        _controller.OnJumpInputs();
        _currentOnAirTimer += Time.deltaTime;
        if (_currentOnAirTimer <= _onAirTimer)
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
        _player.ReturnJumps();
    }

    public void OnFixedUpdate()
    {
        _player.Move(_controller.xAxis);
    }

    public void OnUpdate()
    {
        _controller.OnAirInputs();
        if (_player.GroundCheck.IsGrounded)
            _player.fsm.ChangeState(StateName.Idle);
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
        Helpers.LevelTimerManager.StartLevelTimer();


        _currentDashDuration = 0;
        Vector3 newDashDir = Vector3.one;
        if (_controller.xAxis != 0)
        {
            _dashDirection = _controller.xAxis;
        }
        else
        {
            float angle = _player.WeaponManager.GetAngle();

            if (angle > 90 || angle < -90)
            {
                _dashDirection = -1;
                newDashDir.x = -1;
            }
            else
            {
                _dashDirection = 1;
                newDashDir.x = 1;
            }
        }
        _player.OnDash(newDashDir);

        _player.StopAllCoroutines();
    }

    public void OnExit()
    {
        _player.FreezeVelocity();
        _player.BlockDash();
    }

    public void OnFixedUpdate()
    {
        _player.Dash(_dashDirection);
    }

    public void OnUpdate()
    {
        _controller.OnDashInputs();
        _currentDashDuration += Time.deltaTime;
        if (_currentDashDuration < _player.DashDuration) return;

        _player.fsm.ChangeState(StateName.OnAir);
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
        _player.AlreadyJumped = false;
    }

    public void OnExit()
    {
        _player.NormalGravity();
        _player.ExitClimb?.Invoke();
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