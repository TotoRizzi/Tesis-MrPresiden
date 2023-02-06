using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_GroundHumanoid : Enemy
{
    StateMachine _fsm;
    [SerializeField] protected Animator _anim;

    [SerializeField] protected Transform sprite;
    public Transform Sprite { get { return sprite; } private set { } }


    [SerializeField] Transform[] _wayPoints = null;
    public Transform[] WayPoints { get { return _wayPoints; } private set { } }

    [SerializeField] float _waypointSpeed = 1f;
    public float WaypointSpeed { get { return _waypointSpeed; } private set { } }

    [SerializeField] float _sightRange = 10f;
    public float SightRange { get { return _sightRange; } private set { } }

    [SerializeField] float _stopAttackingTime = 1f;
    public float StopAttackingTime { get { return _stopAttackingTime; } private set { } }


    public override void Start()
    {
        base.Start();

        //Components
        _fsm = new StateMachine();
        _anim = GetComponentInChildren<Animator>();

        //StateMachine
        _fsm.AddState(StateName.SH_Patrol, new SH_PatrolState(_fsm, this));
        _fsm.AddState(StateName.SH_Attack, new SH_AttackState(_fsm, this));
        _fsm.ChangeState(StateName.SH_Patrol);

        OnUpdate += _fsm.Update;
    }

    public Vector3 DistanceToPlayer()
    {
        return ((gameManager.Player.transform.position + transform.up) - transform.position);
    }

    public virtual void OnAttack() { }
    public virtual void OnCancelAttack() { }

    public virtual void OnPatrolStart() { }
    public virtual void OnAttackStart() { }
}

public class SH_PatrolState : IState
{
    StateMachine _fsm;
    Enemy_GroundHumanoid _enemy;
    GameManager gameManager;

    Vector3 _dir;
    int _index;

    public SH_PatrolState(StateMachine fsm, Enemy_GroundHumanoid enemy)
    {
        _fsm = fsm;
        _enemy = enemy;

        gameManager = GameManager.instance;
    }

    public void OnEnter()
    {
        _enemy.OnPatrolStart();
        ChangeDir();
    }

    public void OnExit() 
    {
    }

    public void OnFixedUpdate() { }

    public void OnUpdate()
    {
        Debug.Log("Patrolling");

        //Patrulla y si lo ve al player, lo ataca

        Move();

        if (Physics2D.Raycast(_enemy.transform.position, _enemy.DistanceToPlayer().normalized, _enemy.SightRange, gameManager.PlayerLayer) && 
            !Physics2D.Raycast(_enemy.transform.position, _enemy.DistanceToPlayer().normalized, _enemy.DistanceToPlayer().magnitude, gameManager.GroundLayer))
            _fsm.ChangeState(StateName.SH_Attack);
    }

    void Move()
    {
        _enemy.transform.position += _dir * _enemy.WaypointSpeed * Time.deltaTime;

        if (Vector3.Distance(_enemy.transform.position, _enemy.WayPoints[_index].position) <= .2f) ChangeDir();
    }

    void ChangeDir()
    {
        _index++;

        if (_index > _enemy.WayPoints.Length - 1)
        {
            _index = 0;
        }

        _dir = (_enemy.WayPoints[_index].position - _enemy.transform.position);
        _dir.y = 0;
        _dir.z = 0;

        _dir.Normalize();
        Flip(_dir);
    }

    void Flip(Vector3 dir)
    {
        _enemy.Sprite.right = dir;
    }
}
public class SH_AttackState : IState
{
    StateMachine _fsm;
    Enemy_GroundHumanoid _enemy;
    GameManager gameManager;

    bool _isInCoroutine = false;

    public SH_AttackState(StateMachine fsm, Enemy_GroundHumanoid enemy)
    {
        _fsm = fsm;
        _enemy = enemy;

        gameManager = GameManager.instance;
    }

    public void OnEnter()
    {
        _enemy.OnAttackStart();
        _isInCoroutine = false;
    }

    public void OnExit() 
    {
        _enemy.OnCancelAttack();
    }

    public void OnFixedUpdate() { }

    public void OnUpdate()
    {
        _enemy.OnAttack();

        if (!Physics2D.Raycast(_enemy.transform.position, _enemy.DistanceToPlayer().normalized, _enemy.SightRange, gameManager.PlayerLayer) ||
            Physics2D.Raycast(_enemy.transform.position, _enemy.DistanceToPlayer().normalized, _enemy.DistanceToPlayer().magnitude, gameManager.GroundLayer))
        {
            if (!_isInCoroutine)
            {
                _enemy.StartCoroutine(StopAttackingCoroutine());
                _isInCoroutine = true;
            }
        }
        //else if (_isInCoroutine) _enemy.StopAllCoroutines();
    }

    IEnumerator StopAttackingCoroutine()
    {
        yield return new WaitForSeconds(_enemy.StopAttackingTime);
        _fsm.ChangeState(StateName.SH_Patrol);
    }
}
