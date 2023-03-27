using System.Collections;
using UnityEngine;
public class PresidentFinalAnimation : MonoBehaviour
{
    [SerializeField] Transform _groundCheck;
    Rigidbody2D _rb;
    Animator _anim;
    bool _inGrounded => Physics2D.Raycast(_groundCheck.position, Vector2.down, .1f, GameManager.instance.GroundLayer);

    System.Action _currentAction = delegate { };
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
        _anim.Play("Empty");
    }
    private void Update()
    {
        _currentAction();
    }

    private void JumpState()
    {
        transform.localScale = new Vector2(-1, 1);
        if (_inGrounded)
            _rb.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerFinalAnimation>();
        if (player != null)
        {
            _currentAction = JumpState;
            StartCoroutine(Escape());
            player.PresidentNear();
        }

        if (collision.GetComponent<NextSceneOnTrigger>())
        {
            _anim.Play("Empty");
            _currentAction = delegate { };
        }
    }

    IEnumerator Escape()
    {
        Physics2D.IgnoreLayerCollision(21, 8);
        yield return new WaitForSeconds(3f);
        _currentAction = delegate { };
        yield return new WaitForSeconds(2f);
        _currentAction = Run;
        transform.localScale = new Vector2(1, 1);
    }

    void Run()
    {
        _anim.Play("RunAnimation");
        transform.position += transform.right * 4 * Time.deltaTime;
    }
}
