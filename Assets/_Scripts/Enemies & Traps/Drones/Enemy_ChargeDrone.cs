using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ChargeDrone : Enemy
{
    StateMachine _fsm;

    [Header ("Idle")]
    [SerializeField] float _rotationSpeed;
    public float RotationSpeed { get { return _rotationSpeed; } }

    [SerializeField] float _idleTime;
    public float IdleTime { get { return _idleTime; } }

    [Header("Charge")]
    [SerializeField] float _chargeSpeed;
    public float ChargeSpeed { get { return _chargeSpeed; } }

    [SerializeField] float _chargeDistance;
    public float ChargeDistance { get { return _chargeDistance; } }

    public override void Start()
    {
        base.Start();

        //Components
        _fsm = new StateMachine();

        //StateMachine
        _fsm.AddState(StateName.CD_Idle, new CD_Idle(_fsm, this));
        _fsm.AddState(StateName.CD_Charge, new CD_Charge(_fsm, this));
        _fsm.ChangeState(StateName.CD_Idle);

        OnUpdate += _fsm.Update;
    }

    public Vector3 GetDistanceToPlayer()
    {
        return DistanceToPlayer();
    }
}

public class CD_Idle : IState
{
    GameManager _gameManager;
    StateMachine _fsm;
    Enemy_ChargeDrone _enemy;
    Player _player;

    public CD_Idle(StateMachine fsm, Enemy_ChargeDrone enemy)
    {
        _fsm = fsm;
        _enemy = enemy;

        _gameManager = GameManager.instance;
        _player = _gameManager.Player;
    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate() 
    { 

    }

    public void OnUpdate()
    {
        Vector3 vectorToTarget = _player.transform.position - _enemy.transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        _enemy.transform.rotation = Quaternion.Lerp(_enemy.transform.rotation, q, Time.deltaTime * _enemy.RotationSpeed);


        if (Physics2D.Raycast(_enemy.transform.position, _enemy.transform.right, _enemy.GetDistanceToPlayer().magnitude, _gameManager.PlayerLayer))
        {
            _fsm.ChangeState(StateName.CD_Charge);
        }
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