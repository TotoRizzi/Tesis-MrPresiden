using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_WayPoint : IMovement
{
    Transform[] _wayPoints;
    Transform _transform;
    float _speed;
    int _index;
    Vector3 _dir;

    public Movement_WayPoint(Transform transform, float speed, Transform[] wayPoints)
    {
        _transform = transform;
        _wayPoints = wayPoints;
        _speed = speed;

        ChangeDir();
    }
    public void Move()
    {
        _transform.position += _dir.normalized * _speed * Time.deltaTime;

        if (Vector3.Distance(_transform.position, _wayPoints[_index].position) <= .2f) ChangeDir();
    }

    void ChangeDir()
    {
        _index++;

        if (_index > _wayPoints.Length - 1)
        {
            _index = 0;
        }

        _dir = (_wayPoints[_index].position - _transform.position);
        _dir.y = 0;
        _dir.z = 0;

        _dir.Normalize();
    }
}
