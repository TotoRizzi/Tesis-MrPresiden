using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IA2;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy_Cover : Enemy_Shooting
{
    public enum EnemyStates {CROUCH, CROUCHIDLE, IDLE, RUNTOPLAYER, RUNTOCOVER, STAND }
    public EventFSM<EnemyStates> fsm;
    Animator _anim;
    Rigidbody2D _rb;

    [SerializeField] float _speed;
    [SerializeField] float _rangeToStopFollowingPlayer = 5f;

    public override void Start()
    {
        base.Start();
        
        _anim = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody2D>();

        FsmCreation();    

        OnAttack += Shoot;
        OnUpdate += Flip;
        OnUpdate += fsm.Update;
    }

    public override void Update()
    {
        base.Update();
        //fsm.Update();
    }

    void Shoot()
    {
        FRY_Bullet.Instance.pool.GetObject().SetPosition(bulletSpawnPosition.position)
                                            .SetDirection(sprite.right)
                                            .SetDmg(bulletDamage)
                                            .SetLayer(Layers.EnemyAttack)
                                            .SetSpeed(bulletSpeed);
    }

    void Flip()
    {
        Vector3 newScale = Vector3.one;

        if (gameManager.Player.transform.position.x < transform.position.x)
        {
            sprite.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            sprite.rotation = Quaternion.Euler(0, 0, 0);
        }
        sprite.localScale = newScale;
    }

    public void Move()
    {
        _rb.MovePosition(transform.position + sprite.right * Time.deltaTime * _speed);
    }


    void FsmCreation()
    {
        #region Fsm Creation

        var standingIdle = new State<EnemyStates>("STANDINGIDLE");
        var runToPlayer = new State<EnemyStates>("RUNTOPLAYER");
        var runToCover = new State<EnemyStates>("RUNTOCOVER");
        var crouching = new State<EnemyStates>("CROUCH");

        #endregion

        #region Fsm Transitions

        StateConfigurer.Create(standingIdle)
                       .SetTransition(EnemyStates.RUNTOCOVER, runToCover)
                       .SetTransition(EnemyStates.RUNTOPLAYER, runToPlayer)
                       .Done();

        StateConfigurer.Create(runToCover)
                       .SetTransition(EnemyStates.CROUCH, crouching)
                       .SetTransition(EnemyStates.IDLE, standingIdle)
                       .Done();

        StateConfigurer.Create(runToPlayer)
                       .SetTransition(EnemyStates.IDLE, standingIdle)
                       .Done();

        StateConfigurer.Create(crouching)
                       .SetTransition(EnemyStates.IDLE, standingIdle)
                       .Done();

        #endregion

        #region Fsm Configuration

        standingIdle.OnEnter += x => _anim.Play("Idle");
        standingIdle.OnUpdate += () => CheckCover();

        runToCover.OnEnter += x => _anim.Play("Run");
        runToCover.OnUpdate += () => RunToCover();

        runToPlayer.OnEnter += x => _anim.Play("Run");
        runToPlayer.OnUpdate += () => RunToPlayer();

        crouching.OnEnter += x => _anim.Play("Crouch");
        crouching.OnUpdate += () => CheckToStandUp();

        #endregion

        fsm = new EventFSM<EnemyStates>(standingIdle);
    }

    void CheckCover()
    {
        if (Vector3.Distance(gameManager.Player.transform.position, transform.position) < _rangeToStopFollowingPlayer) return;

        if (Physics2D.Raycast(transform.position, sprite.right, 10f, gameManager.GroundLayer))
            fsm.SendInput(EnemyStates.RUNTOCOVER);
        else
            fsm.SendInput(EnemyStates.RUNTOPLAYER);
    }

    void RunToCover()
    {
        Debug.Log("Run To Cover");
        Move();
        if (Physics2D.Raycast(transform.position, sprite.right, 1f, gameManager.GroundLayer))
            fsm.SendInput(EnemyStates.CROUCH);
        else if (!Physics2D.Raycast(transform.position, sprite.right, (gameManager.Player.transform.position - transform.position).magnitude, gameManager.GroundLayer))
            fsm.SendInput(EnemyStates.IDLE);
    }

    void RunToPlayer()
    {
        Debug.Log("Run To Player");

        Move();
        if(Vector3.Distance(gameManager.Player.transform.position, transform.position) < _rangeToStopFollowingPlayer
          ||
          Physics2D.Raycast(transform.position, sprite.right, (gameManager.Player.transform.position - transform.position).magnitude, gameManager.GroundLayer))
            fsm.SendInput(EnemyStates.IDLE);
    }

    void CheckToStandUp()
    {
        if (!Physics2D.Raycast(transform.position, sprite.right, (gameManager.Player.transform.position - transform.position).magnitude, gameManager.GroundLayer))
            fsm.SendInput(EnemyStates.IDLE);
    }
}
