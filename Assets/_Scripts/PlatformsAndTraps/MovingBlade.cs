using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlade : MonoBehaviour
{
    IMovement _wayPointMovement;

    [SerializeField] float _dmg = 1f;
    [SerializeField] Transform[] _wayPoints;
    [SerializeField] float _speed;

    private void Start()
    {
        _wayPointMovement = new Movement_BasicWayPoint(this.transform, _speed, _wayPoints);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var entity = collision.GetComponent<IDamageable>();

        if(entity != null) entity.TakeDamage(_dmg);
    }
}
