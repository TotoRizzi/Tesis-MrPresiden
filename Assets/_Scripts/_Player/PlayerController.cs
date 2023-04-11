public class PlayerController
{
    Player _player;
    InputManager _inputManager;
    public float xAxis { get; private set; }
    public float yAxis { get; private set; }

    public PlayerController(Player player, PlayerView v)
    {
        _inputManager = InputManager.Instance;

        _player = player;

        _player.OnIdle += v.Idle;
        _player.OnMove += v.Run;
        _player.OnJump += v.Jump;
        _player.OnDash += v.Dash;
    }

    public void OnUpdate()
    {
        xAxis = _inputManager.GetAxisRaw("Horizontal");
        yAxis = _inputManager.GetAxisRaw("Vertical");
    }

    public void IdleInputs()
    {
        if (_inputManager.GetButtonDown("Jump") && _player.CanJump)
            _player.fsm.ChangeState(StateName.Jump);
        else if (xAxis != 0)
            _player.fsm.ChangeState(StateName.Move);
        if (_inputManager.GetButtonDown("Dash") && _player.CanDash)
            _player.fsm.ChangeState(StateName.Dash);
    }

    public void MovingInputs()
    {
        if (_inputManager.GetButtonDown("Jump") && _player.CanJump)
            _player.fsm.ChangeState(StateName.Jump);
        else if (xAxis == 0)
            _player.fsm.ChangeState(StateName.Idle);
        if (_inputManager.GetButtonDown("Dash") && _player.CanDash)
            _player.fsm.ChangeState(StateName.Dash);
    }

    public void OnAirInputs()
    {
        if (_inputManager.GetButtonDown("Dash") && _player.CanDash)
            _player.fsm.ChangeState(StateName.Dash);
        else if (_inputManager.GetButtonDown("Jump") && _player.CanJump)
            _player.fsm.ChangeState(StateName.Jump);
    }

    public void OnJumpInputs()
    {
        if (_inputManager.GetButtonDown("Dash") && _player.CanDash)
            _player.fsm.ChangeState(StateName.Dash);
    }

    public void ClimbingInputs()
    {
        if (_inputManager.GetButtonDown("Dash") && _player.CanDash)
            _player.fsm.ChangeState(StateName.Dash);
        else if (_inputManager.GetButtonDown("Jump") && _player.CanJump)
            _player.fsm.ChangeState(StateName.Jump);
    }
}
