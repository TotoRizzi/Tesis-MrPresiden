using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Moving : MonoBehaviour
{
    IMovement _wayPointMovement;

    [SerializeField] Transform[] _wayPoints;
    [SerializeField] float _speed;

    private void Start()
    {
        _wayPointMovement = new Movement_BasicWayPoint(transform, _speed, _wayPoints, true);
    }

    private void Update()
    {
        _wayPointMovement.Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) collision.gameObject.transform.parent = this.transform;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) collision.gameObject.transform.parent = null;
    }
}
