using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_FallingWall : MonoBehaviour
{
    [SerializeField] bool _horizontal;
    [SerializeField] float _speed;
    [SerializeField] float _waitSeconds;
    Vector3 _dir;
    bool _isFacingRight = true;
    bool _canMove = true;

    public void Start()
    {
        if (!_horizontal)
            FlipVertical();
        else
        {
            FlipHorizontal();
            transform.right = transform.up;
        }
    }

    private void Update()
    {
        if(_canMove) Move();
    }

    IEnumerator Wait()
    {
        _canMove = false;

        yield return new WaitForSeconds(_waitSeconds);

        _canMove = true;
    }

    protected void Move()
    {
        transform.position += _dir * _speed * Time.deltaTime;
    }

    void FlipVertical()
    {
        if (_isFacingRight)
        {
            _isFacingRight = false;

            _dir = -Vector3.up;
        }
        else
        {
            _isFacingRight = true;
            _dir = Vector3.up;

        }
    }

    void FlipHorizontal()
    {
        if (_isFacingRight)
        {
            _isFacingRight = false;

            _dir = -Vector3.right;
        }
        else
        {
            _isFacingRight = true;
            _dir = Vector3.right;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "InvisibleWall")
        {
            Helpers.GameManager.EnemyManager.HeavyAttack();
            StartCoroutine(Wait());
            if (!_horizontal)
                FlipVertical();
            else
                FlipHorizontal();
        }
    }

}