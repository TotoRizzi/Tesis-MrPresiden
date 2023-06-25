using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_FollowDrone : Enemy
{
    IMovement _myMovement;

    [SerializeField] protected float _speed = 1f;

    public override void Start()
    {
        base.Start();

        _myMovement = new Movement_BasicFollowTarget(transform, GameManager.instance.Player.transform, _speed);
        OnUpdate += MoveIfPlayerInSight;
    }

    protected void MoveIfPlayerInSight()
    {
        if (CanSeePlayer()) _myMovement.Move();
    }

    public override void ReturnObject()
    {
        base.ReturnObject();
        FRY_Enemy_FollowDrone.Instance.pool.ReturnObject(this);
    }
}
