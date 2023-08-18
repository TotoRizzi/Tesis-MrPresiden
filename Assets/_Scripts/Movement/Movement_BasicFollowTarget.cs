using System.Collections.Generic;
using UnityEngine;
public class Movement_BasicFollowTarget : IMovement
{
    Transform _transform;
    Transform _target;

    Vector3 _offSet = new Vector3(0, .7f);
    float _speed;
    public Movement_BasicFollowTarget(Transform transform, Transform target, float speed)
    {
        _transform = transform;
        _target = target;
        _speed = speed;
    }

    public void Move()
    {
        Vector3 dir = ((_target.position + _offSet) - _transform.position).normalized;

        _transform.position += dir * _speed * Time.deltaTime;
    }
}

public class PathfindingMovement : IMovement
{
    Transform _transform, _target;
    float _speed;

    AStar _aStar;
    List<Node> _path = new List<Node>();
    Node[] _allNodes;
    Node _startingNode, _goalNode;
    LayerMask _wallLayer;
    public PathfindingMovement(Transform transform, Transform target, float speed)
    {
        _transform = transform;
        _target = target;
        _speed = speed;

        _wallLayer = LayerMask.GetMask("Border");
    }
    public void Move()
    {
        Vector3 dir = _path[0].transform.position - _transform.position;
        _transform.position += dir.normalized * _speed * Time.deltaTime;

        if (dir.magnitude < 0.1f)
            _path.RemoveAt(0);
    }
    public void SetPath()
    {
        _aStar = new AStar();
        if (_path == null) return;
        _path = _aStar.ConstructPathAStar(GetStartingNode(), GetGoalNode(_target.position));
        if (InSight(GetStartingNode().transform.position, _transform.position, _wallLayer)) _path.Remove(GetStartingNode());
    }
    public void SetAllNodes(Node[] allNodes)
    {
        _allNodes = allNodes;
    }
    bool InSight(Vector3 start, Vector3 end, LayerMask layer)
    {
        return Physics2D.Raycast(start, end, layer);
    }
    Node GetStartingNode()
    {
        float minValue = 0;
        for (int i = 0; i < _allNodes.Length; i++)
        {
            float distance = Vector3.Distance(_allNodes[i].transform.position, _transform.position);
            if (minValue == 0)
            {
                minValue = distance;
                _startingNode = _allNodes[i];
            }
            else if (distance < minValue)
            {
                minValue = distance;
                _startingNode = _allNodes[i];
            }
        }
        return _startingNode;
    }
    Node GetGoalNode(Vector3 goalPos)
    {
        float minValue = 0;
        for (int i = 0; i < _allNodes.Length; i++)
        {
            float distance = Vector3.Distance(goalPos, _allNodes[i].transform.position);
            if (minValue == 0)
            {
                minValue = distance;
                _goalNode = _allNodes[i];
            }
            else if (distance < minValue)
            {
                minValue = distance;
                _goalNode = _allNodes[i];
            }
        }
        return _goalNode;
    }
}
