using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy_Shooting : Enemy
{
    protected GameManager gameManager;
    protected Action OnAttack;

    [SerializeField] protected Transform bulletSpawnPosition;
    [SerializeField] protected Transform armPivot;
    [SerializeField] protected Transform sprite;
    [SerializeField] protected Transform weaponSprite;

    [SerializeField] protected float bulletDamage = 1f;
    [SerializeField] protected float bulletSpeed = 10f;
    [SerializeField] protected float attackSpeed = 2f;
    protected float currentAttackSpeed;

    public override void Start()
    {
        base.Start();
        gameManager = GameManager.instance;
        OnUpdate += Attack;
    }

    public virtual void Attack()
    {
        currentAttackSpeed += Time.deltaTime;

        if (currentAttackSpeed > attackSpeed)
        {
            OnAttack();
            currentAttackSpeed = 0;
        }
    }
}
