using System.Collections;
using UnityEngine;
public class PlayerFinalAnimation : MonoBehaviour
{
    [SerializeField] Transform _groundCheck;
    Animator _anim;
    Rigidbody2D _rb;
    bool _inGrounded => Physics2D.Raycast(_groundCheck.position, Vector2.down, .1f, GameManager.instance.GroundLayer);

    bool _president;
    void Start()
    {
        _anim = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (!_president)
            transform.position += transform.right * 4 * Time.deltaTime;
        else
            JumpState();
    }
    private void JumpState()
    {
        _anim.Play("Empty");
        if (_inGrounded)
            _rb.AddForce(Vector2.up * 2,ForceMode2D.Impulse);
    }
    public void PresidentNear()
    {
        _president = true;
        StartCoroutine(GoMenu());
    }
    IEnumerator GoMenu()
    {
        yield return new WaitForSeconds(4f);
        GameManager.instance.SceneManager.GoToMenu();
    }
}
