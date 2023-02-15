using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Waypoint : Enemy
{

    protected IMovement _wayPointMovement;

    [SerializeField] Transform[] _wayPoints; 
    [SerializeField] float _waypointSpeed;
    [SerializeField] bool _static;


    public override void Start()
    {
        base.Start();

        if (_static) return;
        _wayPointMovement = new Movement_BasicTransformWayPoint(this.transform, _waypointSpeed, _wayPoints);

        OnUpdate += _wayPointMovement.Move;
    }
}
