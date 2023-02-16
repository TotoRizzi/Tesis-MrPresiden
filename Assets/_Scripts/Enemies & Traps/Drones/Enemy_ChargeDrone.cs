using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ChargeDrone : Enemy
{
    StateMachine _fsm;

    [Header ("Idle")]
    [SerializeField] float _idleWaitTime;
    public float IdleWaitTime { get { return _idleWaitTime; } }

    [Header("Charge")]
    [SerializeField] float _chargeSpeed;
    public float ChargeSpeed { get { return _chargeSpeed; } }

    [SerializeField] float _chargeDistance;
    public float ChargeDistance { get { return _chargeDistance; } }

    [SerializeField] GameObject _particles;
    public GameObject Particles { get { return _particles; } }

    public override void Start()
    {
        base.Start();

        //Components
        _fsm = new StateMachine();

        //StateMachine
        _fsm.AddState(StateName.CD_Idle, new CD_Idle(_fsm, this));
        _fsm.AddState(StateName.CD_Charge, new CD_Charge(_fsm, this));
        _fsm.ChangeState(StateName.CD_Idle);

        OnUpdate += FalseUpdate;
    }
    void FalseUpdate()
    {
        if(CanSeePlayer()) _fsm.Update();
    }
    public Vector3 GetDistanceToPlayer()
    {
        return DistanceToPlayer();
    }
    public void LookAtPlayer()
    {
        transform.right = GetDistanceToPlayer().normalized;
    }
}

public class CD_Idle : IState
{
    GameManager _gameManager;
    StateMachine _fsm;
    Enemy_ChargeDrone _enemy;
    Player _player;

    float _currentIdleTime;

    public CD_Idle(StateMachine fsm, Enemy_ChargeDrone enemy)
    {
        _fsm = fsm;
        _enemy = enemy;

        _gameManager = GameManager.instance;
        _player = _gameManager.Player;
    }

    public void OnEnter()
    {
        _enemy.Particles.SetActive(false);
        _currentIdleTime = 0;
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate() 
    { 

    }

    public void OnUpdate()
    {
        _currentIdleTime += Time.deltaTime;
        _enemy.LookAtPlayer();

        if(_currentIdleTime > _enemy.IdleWaitTime) _fsm.ChangeState(StateName.CD_Charge);
    }
}
public class CD_Charge : IState
{
    StateMachine _fsm;
    Enemy_ChargeDrone _enemy;
    GameManager _gameManager;

    float _currentChargeDistance;

    public CD_Charge(StateMachine fsm, Enemy_ChargeDrone enemy)
    {
        _fsm = fsm;
        _enemy = enemy;

        _gameManager = GameManager.instance;
    }

    public void OnEnter()
    {
        _enemy.Particles.SetActive(true);

    }

    public void OnExit()
    {
        _currentChargeDistance = 0;
    }

    public void OnFixedUpdate() 
    { 
    
    }

    public void OnUpdate()
    {
        _currentChargeDistance += Time.deltaTime;
        if (_currentChargeDistance > _enemy.ChargeDistance || Physics2D.Raycast(_enemy.transform.position, _enemy.transform.right, .6f, _gameManager.BorderLayer))
            _fsm.ChangeState(StateName.CD_Idle);

        Charge();
    }
    void Charge()
    {
        _enemy.transform.position += _enemy.transform.right * _enemy.ChargeSpeed * Time.deltaTime;
    }
}