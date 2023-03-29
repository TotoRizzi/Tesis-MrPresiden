using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_KamikazeRobot : Enemy_Waypoint
{
    [SerializeField] float _dropSpeed;
    [SerializeField] float _overlapCircleRadius = 1.5f;
    [SerializeField] float _dmg;

    bool _isDropping;

    public override void Start()
    {
        base.Start();
        OnUpdate += Attack;
    }

    void Drop()
    {
        transform.position += -transform.up * _dropSpeed * Time.deltaTime;
        Debug.Log("Dropping");
    }

    public void Attack()
    {
        OnUpdate -= Drop;
        if (Physics2D.Raycast(transform.position, -Vector2.up, 10f, gameManager.PlayerLayer))
        {
            _isDropping = true;
            OnUpdate += Drop;
            OnUpdate -= Move;
            OnUpdate -= Attack;
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
        Debug.Log(!_isDropping || collision.tag == "Bullet");
        if (!_isDropping || collision.tag == "Bullet") return;
            Die();
    }
    public override void ReturnObject()
    {
        base.ReturnObject();
        FRY_Enemy_KamikazeRobot.Instance.pool.ReturnObject(this);
    }
    public override void Reset()
    {
        OnUpdate += Attack;

        if (_isDropping)
        {
            _isDropping = false;
            OnUpdate += Move;
        }
        base.Reset();
    }
}
