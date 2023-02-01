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
    public enum PlayerInputs { STANDINGMOVE, JUMP, STANDINGIDLE, ONAIR, CLIMBING, CROUCH, CROUCHWALKING, CROUCHIDLE, STAND}
    public EventFSM<PlayerInputs> fsm;

    PlayerController _controller;
    Rigidbody2D _rb;
    BoxCollider2D _collider;
    WeaponManager _weaponManager;

    [Header("Movement")]
    [SerializeField] Transform _playerArm;
    [SerializeField] Transform _playerSprite;
    [SerializeField] Vector3 _crouchColliderSize;
    [SerializeField] Vector3 _crouchColliderOffset;
    Vector3 _colliderDefaultSize;
    Vector3 _colliderDefaultOffset;
    Vector3 _playerDefaultSpriteSize;
    [SerializeField] float _standingSpeed;
    [SerializeField] float _crouchingSpeed;
  
    [Header("Attack")]
    [SerializeField] Transform _bulletSpawnPosition;
    [SerializeField] float _attackSpeed;
    bool _canAttack = true;

    [Header("Jump")]

    [SerializeField] float _coyoteTime = .5f;
    public float CoyoteTime { get { return _coyoteTime; } }

    GroundCheck _groundCheck;
    [SerializeField] float _jumpForce = 5;
    [SerializeField] int _maxJumps = 1;

    int _currentJumps = 1;
    bool _canJump => _currentJumps > 0;
    float _defaultGravity;

    public event Action OnIdle;
    public event Action OnRun;
    public event Action OnCrouch;
    public event Action OnCrouchIdle;
    public event Action OnCrouchRun;


    private void Start()
    {
        _controller = new PlayerController(this, GetComponent<PlayerView>());
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _weaponManager = GetComponent<WeaponManager>();
        _groundCheck = GetComponentInChildren<GroundCheck>();

        _currentJumps = _maxJumps;

        _playerDefaultSpriteSize = _playerSprite.transform.localScale;
        _colliderDefaultSize = _collider.size;
        _colliderDefaultOffset = _collider.offset;

        _defaultGravity = _rb.gravityScale;

        FsmCreation();
    }

    private void Update()
    {
        fsm.Update();
        _controller.OnUpdate();
    }
    private void FixedUpdate()
    {
        fsm.FixedUpdate();
    }

    void FsmCreation()
    {
        #region Fsm Creation
        var standingIdle = new State<PlayerInputs>("STANDINGIDLE");
        var standingMoving = new State<PlayerInputs>("STANDINGMoving");
        var jumping = new State<PlayerInputs>("Jumping");
        var onAir = new State<PlayerInputs>("ONAIR");
        var climbing = new State<PlayerInputs>("CLIMB");
        var crouching = new State<PlayerInputs>("CROUCH");
        var crouchWalking = new State<PlayerInputs>("CROUCHWALKING");
        var crouchIdle = new State<PlayerInputs>("CROUCHIDLE");
        var stand = new State<PlayerInputs>("STAND");

        #endregion

        #region Fsm Transitions
        StateConfigurer.Create(standingIdle)
                       .SetTransition(PlayerInputs.STANDINGMOVE, standingMoving)
                       .SetTransition(PlayerInputs.JUMP, jumping)
                       .SetTransition(PlayerInputs.CLIMBING, climbing)
                       .SetTransition(PlayerInputs.CROUCH, crouching)
                       .Done();

        StateConfigurer.Create(standingMoving)
                       .SetTransition(PlayerInputs.STANDINGIDLE, standingIdle)
                       .SetTransition(PlayerInputs.JUMP, jumping)
                       .SetTransition(PlayerInputs.CLIMBING, climbing)
                       .SetTransition(PlayerInputs.CROUCH, crouching)
                       .Done();

        StateConfigurer.Create(jumping)
                       .SetTransition(PlayerInputs.ONAIR, onAir)
                       .Done();

        StateConfigurer.Create(onAir)
                       .SetTransition(PlayerInputs.STANDINGIDLE, standingIdle)
                       .SetTransition(PlayerInputs.CLIMBING, climbing)
                       .Done();

        StateConfigurer.Create(climbing)
                       .SetTransition(PlayerInputs.JUMP, jumping)
                       .SetTransition(PlayerInputs.STANDINGIDLE, standingIdle)
                       .SetTransition(PlayerInputs.STANDINGMOVE, standingMoving)
                       .Done();

        StateConfigurer.Create(crouching)
                       .SetTransition(PlayerInputs.CROUCHIDLE, crouchIdle)
                       .Done();

        StateConfigurer.Create(crouchWalking)
                       .SetTransition(PlayerInputs.CROUCHIDLE, crouchIdle)
                       .SetTransition(PlayerInputs.STAND, stand)
                       .Done();

        StateConfigurer.Create(crouchIdle)
                       .SetTransition(PlayerInputs.CROUCHWALKING, crouchWalking)
                       .SetTransition(PlayerInputs.STAND, stand)
                       .Done();

        StateConfigurer.Create(stand)
                       .SetTransition(PlayerInputs.STANDINGIDLE, standingIdle)
                       .Done();
        #endregion

        #region Fsm Configuration

        standingIdle.OnEnter += x => OnIdle();
        standingIdle.OnUpdate += () => _controller.StandingIdleInputs();

        standingMoving.OnEnter += x => OnRun();
        standingMoving.OnFixedUpdate += () =>
        {
            _controller.StandingGroundMovingInputs();
            Move(_controller.xAxis);
        };

        climbing.OnEnter += x =>
        {
            _rb.gravityScale = 0;
            _rb.velocity = Vector2.zero;
        };
        climbing.OnUpdate += () =>
        {
            _controller.ClimbMovingInputs();
        };
        climbing.OnFixedUpdate += () =>
        {
            ClimbMove(_controller.yAxis);
        };
        climbing.OnExit += x =>
        {
            _rb.gravityScale = _defaultGravity;
            _currentJumps = _maxJumps;
        };

        jumping.OnEnter += x =>
        {
            _rb.velocity = Vector2.zero;
            Jump();
            fsm.SendInput(PlayerInputs.ONAIR);
        };

        onAir.OnUpdate += () =>
        {
            if(_groundCheck.IsGrounded)
            {
                _currentJumps = _maxJumps;
                fsm.SendInput(PlayerInputs.STANDINGIDLE);
            }
            Move(_controller.xAxis);
        };

        crouching.OnEnter += x =>
        {
            OnCrouch();
            _collider.offset = _crouchColliderOffset;
            _collider.size = _crouchColliderSize;
            fsm.SendInput(PlayerInputs.CROUCHIDLE);
        };

        crouchIdle.OnEnter += x => OnCrouchIdle();
        crouchIdle.OnUpdate += () => _controller.CrouchingIdleInputs();

        crouchWalking.OnEnter += x => OnCrouchRun();
        crouchWalking.OnFixedUpdate += () =>
        {
            _controller.CrouchingGroundMovingInputs();
            CrouchMove(_controller.xAxis);
        };

        stand.OnEnter += x =>
        {
            _collider.offset = _colliderDefaultOffset;
            _collider.size = _colliderDefaultSize;
            fsm.SendInput(PlayerInputs.STANDINGIDLE);
        };
        #endregion

        fsm = new EventFSM<PlayerInputs>(standingIdle);
    }

    public void ClimbMove(float yAxis)
    {
        _rb.velocity = new Vector2(_rb.velocity.x, yAxis * _standingSpeed * Time.fixedDeltaTime);
    }

    public void Move(float axis)
    {
        _rb.velocity = new Vector2(axis * _standingSpeed * Time.fixedDeltaTime, _rb.velocity.y);
    }
    public void CrouchMove(float axis)
    {
        _rb.velocity = new Vector2(axis * _crouchingSpeed * Time.fixedDeltaTime, _rb.velocity.y);
    }

    public void Jump()
    {
        if (!_canJump) return;

        _currentJumps--;
        _rb.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);
    }

    public void AddExtraJump()
    {
        _maxJumps++;
    }

    //public void Attack()
    //{
    //    if (_canAttack)
    //    {
    //        FRY_Bullet.Instance.pool.GetObject().SetPosition(_bulletSpawnPosition.position)
    //                                            .SetRotation(_playerArm.rotation)
    //        /*HardCodeado pq es placeHolder*/   .SetDmg(1f)       
    //                                            .SetLayer(Layers.PlayerAttack)
    //        /*HardCodeado pq es placeHolder*/   .SetSpeed(20);
    //
    //        StartCoroutine(ReturnShootCd());
    //    }
    //}

    IEnumerator ReturnShootCd()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_attackSpeed);
        _canAttack = true;
    }
    public void LookAtMouse()
    {
        Vector3 playerLocalScale = _playerDefaultSpriteSize;
        float angle = _weaponManager.GetAngle();

        if (angle > 90 || angle < -90)
        {
            playerLocalScale.x = -_playerDefaultSpriteSize.x;
        }
        else
        {
            playerLocalScale.x = _playerDefaultSpriteSize.x;
        }

        //_playerArm.transform.eulerAngles = new Vector3(0, 0, angle);
        _playerSprite.localScale = playerLocalScale;
    }
    Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    Vector2 GetDirToMousePosition(Transform transform)
    {
        Vector3 mousePosition = GetMousePosition() - (Vector2)transform.position;
        mousePosition.Normalize();

        return mousePosition;
    }
}
