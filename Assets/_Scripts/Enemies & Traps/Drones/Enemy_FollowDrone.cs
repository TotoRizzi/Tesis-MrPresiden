using UnityEngine;
public class Enemy_FollowDrone : Enemy
{
    enum DroneStates { Idle, Follow, Pathfinding };
    [SerializeField] protected float _speed = 1f;

    EventFSM<DroneStates> _myFsm;
    public override void Start()
    {
        base.Start();
        Movement_BasicFollowTarget followTarget = new Movement_BasicFollowTarget(transform, Helpers.GameManager.Player.transform, _speed);
        PathfindingMovement pathfindingMovement = new PathfindingMovement(transform, Helpers.GameManager.Player.transform, _speed);

        var idle = new State<DroneStates>("Idle");
        var follow = new State<DroneStates>("Follow");
        var pathfinding = new State<DroneStates>("Pathfinding");

        StateConfigurer.Create(idle).SetTransition(DroneStates.Follow, follow).Done();
        StateConfigurer.Create(follow).SetTransition(DroneStates.Pathfinding, pathfinding).Done();
        StateConfigurer.Create(pathfinding).SetTransition(DroneStates.Idle, idle).SetTransition(DroneStates.Follow, follow).Done();

        idle.OnUpdate += delegate { if (CanSeePlayer()) _myFsm.SendInput(DroneStates.Follow); };
        follow.OnUpdate += delegate
        {
            if (!CanSeePlayer()) _myFsm.SendInput(DroneStates.Pathfinding);
            followTarget.Move();
        };

        float counter = 0;
        pathfinding.OnEnter += x =>
        {
            pathfindingMovement.SetAllNodes(FindObjectsOfType<Node>());
            pathfindingMovement.SetPath();
        };

        pathfinding.OnUpdate += delegate
        {
            pathfindingMovement.Move();
            counter += Time.deltaTime;
            if (counter >= 3f) _myFsm.SendInput(DroneStates.Idle);

            if (CanSeePlayer()) _myFsm.SendInput(DroneStates.Follow);
        };
        pathfinding.OnExit += x => counter = 0;

        _myFsm = new EventFSM<DroneStates>(idle);
    }
    public override void Update()
    {
        _myFsm?.Update();
    }

    public override void ReturnObject()
    {
        base.ReturnObject();
        FRY_Enemy_FollowDrone.Instance.pool.ReturnObject(this);
    }
}
