using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerJetPack : GeneralPlayer
{
    Action OnUpdate;
    Rigidbody2D _rb;
    [HideInInspector] public StateMachine fsm;
    PlayerJetPackController _controller;


    WeaponManager _weaponManager;
    public WeaponManager WeaponManager { get { return _weaponManager; } private set { } }

    [SerializeField] Transform _playerSprite;

    [SerializeField] float _speed = 400f;
    [SerializeField] float _maxDelayCanMove = .2f;
    float _defaultGravity;

    Vector3 _playerDefaultSpriteSize;


    private void Start()
    {
        StartCoroutine(CanMoveDelay());

        _rb = GetComponent<Rigidbody2D>();
        _weaponManager = GetComponent<WeaponManager>();
        fsm = new StateMachine();
        _controller = new PlayerJetPackController(this);

        OnUpdate += fsm.Update;
        OnUpdate += _controller.OnUpdate;
        OnUpdate += LookAtMouse;

        fsm.AddState(StateName.FlyingUp, new UpState(this, _controller));
        fsm.AddState(StateName.Droping, new DownState(this, _controller));
        fsm.ChangeState(StateName.FlyingUp);

        _playerDefaultSpriteSize = _playerSprite.localScale;
        _defaultGravity = _rb.gravityScale;
    }

    private void Update()
    {
        if (_canMove) OnUpdate?.Invoke();
    }
    private void FixedUpdate()
    {
        if (_canMove) fsm.FixedUpdate();
    }

    public void Move(float axis)
    {
        _rb.velocity = new Vector2(axis * _speed * Time.fixedDeltaTime, _rb.velocity.y);
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

    public IEnumerator CanMoveDelay()
    {
        yield return new WaitForSeconds(_maxDelayCanMove);

        _canMove = true;
    }

    public void CeroGravity()
    {
        _rb.gravityScale = 0;
    }

    public void NormalGravity()
    {
        _rb.gravityScale = _defaultGravity;
    }

    public void FreezeVelocity(bool xAxis = false)
    {
        if (xAxis) _rb.velocity = new Vector2(0, _rb.velocity.y);
        else _rb.velocity = Vector2.zero;
        //OnIdle();
    }

    public void LookAtMouse()
    {
        Vector3 playerLocalScale = _playerDefaultSpriteSize;
        float angle = _weaponManager.GetAngle();

        if (angle > 90 || angle < -90)
            playerLocalScale.x = -_playerDefaultSpriteSize.x;
        else
            playerLocalScale.x = _playerDefaultSpriteSize.x;

        _playerSprite.localScale = playerLocalScale;
    }
}

public class UpState : IState
{
    PlayerJetPack _player;
    PlayerJetPackController _controller;

    public UpState(PlayerJetPack player, PlayerJetPackController controller)
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
        _controller.FlyingInputs();
    }
}
public class DownState : IState
{
    PlayerJetPack _player;
    PlayerJetPackController _controller;

    public DownState(PlayerJetPack player, PlayerJetPackController controller)
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
        _controller.FlyingInputs();
    }
}

public class PlayerJetPackController
{
    PlayerJetPack _player;
    InputManager _inputManager;
    public float xAxis { get; private set; }
    public float yAxis { get; private set; }

    public PlayerJetPackController(PlayerJetPack player)
    {
        _inputManager = InputManager.Instance;

        _player = player;
    }

    public void OnUpdate()
    {
        xAxis = _inputManager.GetAxisRaw("Horizontal");
        yAxis = _inputManager.GetAxisRaw("Vertical");
        Debug.Log("Working");
    }

    public void FlyingInputs()
    {
        if (_inputManager.GetButtonDown("Jump"))
        {
            //Arranque a subir
        }
        else if (_inputManager.GetButtonUp("Jump"))
        {
            //Arranque a bajar
        }
    }
}