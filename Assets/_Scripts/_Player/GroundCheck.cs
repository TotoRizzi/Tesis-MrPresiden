using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private bool _isGrounded;
    public bool IsGrounded { get { return _isGrounded; } private set { } }

    private Player _player;

    private void Start()
    {
        _player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StartCoroutine(CoyoteJump());
    }

    IEnumerator CoyoteJump()
    {
        yield return new WaitForSeconds(_player.CoyoteTime);
        _isGrounded = false;
    }
}
