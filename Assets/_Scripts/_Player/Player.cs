using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IA2;
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

    [SerializeField] int _maxJumps = 1;
    public int MaxJumps { get { return _maxJumps; } private set { } }

    public int _currentJumps = 1;

    bool _canJump => _currentJumps > 0;

    public bool CanJump { get { return _canJump; } private set { } }
    #endregion

    #region Dash
    [Header("Dash")]
    [SerializeField] float _dashSpeed;
    [SerializeField] float _dashDuration;
    public float DashDuration { get { return _dashDuration; } private set { } }

    bool _canDash = true;
    public bool CanDash { get { return _canDash; } private set { } }

    #endregion

    #region View
    public Action OnIdle;
    public Action OnMove;
    public Action<Vector3> OnDash;
    public Action OnJump;
    public Action OnStaminaTick;
    #endregion

    Dictionary<string, Action> _tutorialActions = new Dictionary<string, Action>();
    private void Start()
    {
        _tutorialActions["Dash"] = () => { if (_canDash) fsm.ChangeState(StateName.Dash); };
        _tutorialActions["Jump"] = () => { if (_canJump) fsm.ChangeState(StateName.Jump); };
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
        _maxJumps = 1;
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

        EventManager.SubscribeToEvent(Contains.PLAYER_TUTORIAL_ACTION, UnPauseTutorialAction);
    }

    private void Update()
    {
        if (_canMove) OnUpdate?.Invoke();
    }

    private void FixedUpdate()
    {
        if (_canMove) fsm.FixedUpdate();
    }
    private void OnDestroy()
    {
        EventManager.UnSubscribeToEvent(Contains.PLAYER_TUTORIAL_ACTION, UnPauseTutorialAction);
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

        _groundCheck.Jumped();
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
        _currentJumps = MaxJumps;
        _canDash = true;
    }

    public void BlockDash()
    {
        _canDash = false;
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

    void UnPauseTutorialAction(params object[] param)
    {
        var actionName = (string)param[0];
        if (_tutorialActions.ContainsKey(actionName)) _tutorialActions[actionName]();
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
        {
            _player.fsm.ChangeState(StateName.OnAir);
        }
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
        if (_player.GroundCheck.IsGrounded)
            _player.fsm.ChangeState(StateName.Idle);
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