using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_GroundHumanoid : Enemy
{
    StateMachine _fsm;
    [SerializeField] protected Animator _anim;

    public Transform Sprite { get { return sprite; } private set { } }

    [SerializeField] float _speed = 1f;
    public float Speed { get { return _speed; } private set { } }

    [SerializeField] float _sightRange = 10f;
    public float SightRange { get { return _sightRange; } private set { } }

    [SerializeField] float _stopAttackingTime = 1f;
    public float StopAttackingTime { get { return _stopAttackingTime; } private set { } }

    public bool isFacingRight = true;

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

    public bool GetCanSeePlayer()
    {
        return CanSeePlayer();
    }

    public virtual void OnAttack() { }
    public virtual void OnCancelAttack() { }

    public virtual void OnPatrolStart() { }
    public virtual void OnAttackStart() { }

    public override void ReturnObject()
    {
        base.ReturnObject();
        _fsm.ChangeState(StateName.SH_Patrol);
    }
}

public class SH_PatrolState : IState
{
    StateMachine _fsm;
    Enemy_GroundHumanoid _enemy;

    Vector3 _dir;

    public SH_PatrolState(StateMachine fsm, Enemy_GroundHumanoid enemy)
    {
        _fsm = fsm;
        _enemy = enemy;
    }

    public void OnEnter()
    {
        _enemy.OnPatrolStart();
        _dir = Vector3.right;
    }

    public void OnExit() 
    {
    }

    public void OnFixedUpdate() { }

    public void OnUpdate()
    {
        Move();

        if(_enemy.GetCanSeePlayer())
            _fsm.ChangeState(StateName.SH_Attack);
    }

    void Move()
    {
        _enemy.transform.position += _dir * _enemy.Speed * Time.deltaTime;

        if (Physics2D.Raycast(_enemy.transform.position, _enemy.transform.localScale, 1f, Helpers.GameManager.InvisibleWallLayer)) Flip();
        Debug.DrawLine(_enemy.transform.position, _enemy.Sprite.right);
    }

    void Flip()
    {
        Vector3 newScale = Vector3.one;
        
        if (_enemy.isFacingRight)
        {
            _enemy.isFacingRight = false;
            newScale.x = -1;

            _dir = -Vector3.right;
        }
        else
        {
            _enemy.isFacingRight = true;
            newScale.x = 1;
            _dir = Vector3.right;

        }

        _enemy.transform.localScale = newScale;
    }
}
public class SH_AttackState : IState
{
    StateMachine _fsm;
    Enemy_GroundHumanoid _enemy;

    bool _isInCoroutine = false;

    public SH_AttackState(StateMachine fsm, Enemy_GroundHumanoid enemy)
    {
        _fsm = fsm;
        _enemy = enemy;
    }

    public void OnEnter()
    {
        _enemy.OnAttackStart();
        _isInCoroutine = false;

        //Si no lo seteas a uno se rompe el brazo y apunta para atras por como esta seteado el flip del patrol
        _enemy.transform.localScale = Vector3.one;

    }

    public void OnExit() 
    {
        _enemy.OnCancelAttack();

    }

    public void OnFixedUpdate() { }

    public void OnUpdate()
    {
        _enemy.OnAttack();

        if(!_enemy.GetCanSeePlayer())
        {
            if (!_isInCoroutine)
            {
                _enemy.StartCoroutine(StopAttackingCoroutine());
                _isInCoroutine = true;
            }
        }
    }

    IEnumerator StopAttackingCoroutine()
    {
        yield return new WaitForSeconds(_enemy.StopAttackingTime);
        _fsm.ChangeState(StateName.SH_Patrol);
    }
}
