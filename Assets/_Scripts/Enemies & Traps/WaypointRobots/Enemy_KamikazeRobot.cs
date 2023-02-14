using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_KamikazeRobot : Enemy_Waypoint
{
    [SerializeField] float _dropSpeed;
    [SerializeField] float _overlapCircleRadius = 1.5f;
    [SerializeField] float _dmg;
    [SerializeField] bool _static;

    public override void Start()
    {
        base.Start();
        if(!_static)OnUpdate += Attack;
    }

    void Drop()
    {
        transform.position += -transform.up * _dropSpeed * Time.deltaTime;
    }

    public void Attack()
    {
        if (Physics2D.Raycast(transform.position, -Vector2.up, 8f, gameManager.PlayerLayer))
        {
            OnUpdate += Drop;
            OnUpdate -= _wayPointMovement.Move;
        }
    }
    public override void Die()
    {
        var player = Physics2D.OverlapCircle(transform.position, _overlapCircleRadius, gameManager.PlayerLayer);

        if (player)
            player.GetComponent<IDamageable>().TakeDamage(_dmg);

        base.Die();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag != "Bullet")
            Die();
    }
}
