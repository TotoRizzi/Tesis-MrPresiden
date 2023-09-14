using UnityEngine;
public class Enemy_KamikazeRobot : Enemy
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
        if (Physics2D.CircleCast(transform.position, 1, -Vector3.up, 10f, gameManager.PlayerLayer))
        {
            if (!Physics2D.Raycast(transform.position, -Vector3.up, (transform.position - Helpers.GameManager.Player.transform.position).magnitude, gameManager.GroundLayer))
            {
                _isDropping = true;
                OnUpdate += Drop;
                OnUpdate -= Attack;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _overlapCircleRadius);
    }
    public override void Die()
    {
        base.Die();
        var player = Physics2D.OverlapCircle(transform.position, _overlapCircleRadius, gameManager.PlayerLayer);
        bool inSight = Physics2D.Raycast(transform.position, DistanceToPlayer(), _overlapCircleRadius, Helpers.GameManager.PlayerLayer);
        if (player && inSight)
            player.GetComponent<IDamageable>().TakeDamage(_dmg);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isDropping || collision.CompareTag("Bullet") || collision.gameObject.layer == 25) return;
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
        }
        base.Reset();
    }
}
