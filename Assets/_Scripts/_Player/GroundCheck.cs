using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private bool _isGrounded;
    public bool IsGrounded { get { return _isGrounded; } private set { } }

    private void Start()
    {
        Helpers.GameManager.OnPlayerDead += () => _isGrounded = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _isGrounded = false;
    }
}
