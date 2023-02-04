using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_KamikazeRobot : Enemy_RobotWaypoint
{
    [SerializeField] float _dropSpeed;

    public override void Start()
    {
        base.Start();
        OnUpdate += Attack;
    }

    void Drop()
    {
        transform.position += -transform.up * _dropSpeed * Time.deltaTime;
    }

    public override void Attack()
    {
        if (Physics2D.Raycast(transform.position, -Vector2.up, 8f, gameManager.PlayerLayer))
        {
            OnUpdate += Drop;
            OnUpdate -= _wayPointMovement.Move;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag != "Bullet")
            Die();
    }
}
