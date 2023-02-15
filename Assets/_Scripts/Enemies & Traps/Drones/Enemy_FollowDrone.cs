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

    public Enemy_FollowDrone SetPosition(Vector3 pos)
    {
        transform.position = pos;
        return this;
    }

    private void Reset()
    {
        ResetHp();
        if(gameManager) gameManager.EnemyManager.AddEnemy(this);
    }
    public static void TurnOn(Enemy_FollowDrone b)
    {
        b.Reset();
        b.gameObject.SetActive(true);
    }

    public static void TurnOff(Enemy_FollowDrone b)
    {
        b.gameObject.SetActive(false);
    }
    void ReturnObject()
    {
        gameManager.EnemyManager.RemoveEnemy(this);
        gameManager.EffectsManager.HumanoindKilled(transform.position);
        FRY_FollowDrone.Instance.ReturnObject(this);
    }
    public override void Die()
    {
        ReturnObject();
    }
}
