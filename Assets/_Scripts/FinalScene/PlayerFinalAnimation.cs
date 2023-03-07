using System.Collections;
using UnityEngine;
public class PlayerFinalAnimation : MonoBehaviour
{
    [SerializeField] Transform _groundCheck;
    Animator _anim;
    Rigidbody2D _rb;
    bool _inGrounded => Physics2D.Raycast(_groundCheck.position, Vector2.down, .1f, GameManager.instance.GroundLayer);

    bool _president;

    System.Action _currentAction;
    void Start()
    {
        _anim = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _currentAction = RunState;
    }
    void Update()
    {
        _currentAction();
    }
    void RunState()
    {
        _anim.Play("Run");
        transform.position += transform.right * 4 * Time.deltaTime;
    }
    private void JumpState()
    {
        _anim.Play("Empty");
        if (_inGrounded)
            _rb.AddForce(Vector2.up * 15,ForceMode2D.Impulse);
    }
    public void PresidentNear()
    {
        _currentAction = JumpState;
        StartCoroutine(Escape());
    }
    IEnumerator Escape()
    {
        yield return new WaitForSeconds(3f);
        _currentAction = delegate { };
        yield return new WaitForSeconds(1f);
        _currentAction = RunState;
    }
}
