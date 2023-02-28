using UnityEngine;
public class PresidentFinalAnimation : MonoBehaviour
{
    [SerializeField] Transform _groundCheck;
    Rigidbody2D _rb;
    bool _player;
    bool _inGrounded => Physics2D.Raycast(_groundCheck.position, Vector2.down, .1f, GameManager.instance.GroundLayer);
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (_player)
            JumpState();
    }

    private void JumpState()
    {
        if (_inGrounded)
            _rb.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerFinalAnimation>();
        if (player != null)
        {
            Debug.Log("Player");
            _player = true;
            player.PresidentNear();
        }
    }
}
