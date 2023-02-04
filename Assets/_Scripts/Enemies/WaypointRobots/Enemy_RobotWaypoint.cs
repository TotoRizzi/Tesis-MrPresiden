using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_RobotWaypoint : Enemy
{
    protected GameManager gameManager;

    protected IMovement _wayPointMovement;

    [SerializeField] Transform[] _wayPoints; 
    [SerializeField] float _waypointSpeed;

    public override void Start()
    {
        base.Start();

        _wayPointMovement = new Movement_WayPoint(this.transform, _waypointSpeed, _wayPoints);

        OnUpdate += _wayPointMovement.Move;
        gameManager = GameManager.instance;
    }

    public virtual void Attack()
    {

    }
}
