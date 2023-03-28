using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_FollowDrone : Enemy
{
    IMovement _myMovement;

    [SerializeField] float speed = 1f;

    public override void Start()
    {
        base.Start();

        _myMovement = new Movement_BasicFollowTarget(transform, GameManager.instance.Player.transform, speed);
        OnUpdate += _myMovement.Move;
    }

    public override void ReturnObject()
    {
        base.ReturnObject();
        FRY_Enemy_FollowDrone.Instance.pool.ReturnObject(this);
    }
}
