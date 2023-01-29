using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_RobotWaypoint : Enemy
{
    protected GameManager gameManager;

    [SerializeField] Transform[] _wayPoints;
    Vector3 _dir;
    
    [SerializeField] float _waypointSpeed;
    int _currentWaypoint = 0;

    public override void Start()
    {
        base.Start();

        ChangeDir();

        OnUpdate += Move;
        gameManager = GameManager.instance;
    }

    protected void Move()
    {
        transform.position += _dir.normalized * _waypointSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, _wayPoints[_currentWaypoint].position) <= .2f) ChangeDir();
    }

    void ChangeDir()
    {
        _currentWaypoint++; 

        if(_currentWaypoint > _wayPoints.Length - 1)
        {
            _currentWaypoint = 0;
        }

        _dir = (_wayPoints[_currentWaypoint].position - transform.position);
        _dir.y = 0;
        _dir.z = 0;

        _dir.Normalize();
    }

    public virtual void Attack()
    {

    }
}
