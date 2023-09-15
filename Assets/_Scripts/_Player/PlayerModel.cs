using UnityEngine;
public class PlayerModel
{
    float _currentJumps, _maxJumps;
    LayerMask _groundLayer;
    float _dashTimer, _dashCooldown = .2f;
    float _coyotaTimer;
    float _coyotaTime;
    bool _secondJump;

    #region Constructor 

    Rigidbody2D _rb;
    Transform _myTransform, _spriteContainerTransform, _groundCheckTransform;
    float _speed, _jumpForce, _dashSpeed, _defaultGravity;
    WeaponManager _weaponManager;

    #endregion
    public int GetLookingForDir => _weaponManager.GetAngle() < -90 || _weaponManager.GetAngle() > 90 ? -1 : 1;

    bool InGrounded => Physics2D.OverlapCircle(_groundCheckTransform.position, .2f, _groundLayer);
    public bool CanJump => InGrounded || _coyotaTimer > 0 || _secondJump && _currentJumps <= _maxJumps;
    public bool CanDash => _dashTimer >= _dashCooldown;
    public PlayerModel(Rigidbody2D rb, Transform myTransform, Transform spriteContainerTransform, Transform groundCheckTransform,
        float speed, float jumpForce, float maxJumps, float dashSpeed, float defaultGravity, float coyotaTime, WeaponManager weaponManager)
    {
        _rb = rb;
        _myTransform = myTransform;
        _spriteContainerTransform = spriteContainerTransform;
        _groundCheckTransform = groundCheckTransform;
        _speed = speed;
        _jumpForce = jumpForce;
        _maxJumps = maxJumps;
        _dashSpeed = dashSpeed;
        _defaultGravity = defaultGravity;
        _coyotaTime = coyotaTime;
        _weaponManager = weaponManager;

        _rb.gravityScale = defaultGravity;
        _dashTimer = _dashCooldown;
        _groundLayer = LayerMask.GetMask("Border") + LayerMask.GetMask("Ground");
    }
    public void OnUpdate()
    {
        LookAtMouse();
        if (InGrounded)
        {
            if (_rb.velocity.y <= 0 && _currentJumps != 0) _currentJumps = 0;
            _coyotaTimer = _coyotaTime;
        }
        else _coyotaTimer -= Time.deltaTime;

        _dashTimer += Time.deltaTime;
    }
    public void Move(float axis)
    {
        _rb.velocity = new Vector2(axis * _speed * Time.fixedDeltaTime, _rb.velocity.y);
    }

    public void ClimbMove(float yAxis)
    {
        _rb.velocity = new Vector2(_rb.velocity.x, yAxis * _speed * Time.fixedDeltaTime);
    }
    public void Jump()
    {
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode2D.Impulse);
        _currentJumps++;
        _coyotaTimer = 0;
        _secondJump = false;
    }

    public void Dash(float xAxis)
    {
        _rb.velocity = new Vector2(xAxis * _dashSpeed * Time.fixedDeltaTime, 0f);
        _dashTimer = 0;
        _secondJump = true;
    }

    public void LookAtMouse()
    {
        float angle = _weaponManager.GetAngle();
        Vector3 playerLocalScale = Vector3.one;

        if (angle > 90 || angle < -90)
            playerLocalScale.x *= -1;
        else
            playerLocalScale.x *= Mathf.Abs(1);

        _spriteContainerTransform.localScale = playerLocalScale;
    }
    public void CeroGravity() { _rb.gravityScale = 0; }
    public void NormalGravity() { _rb.gravityScale = _defaultGravity; }
    public void ResetStats()
    {
        _dashTimer = _dashCooldown;
        _currentJumps = 0;
        _coyotaTimer = _coyotaTime;
        _secondJump = false;
    }
}