using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DamageOnTrigger : MonoBehaviour
{
    IDamageable _player;
    [SerializeField] float _dmg;

    private void Start()
    {
        _player = GameManager.instance.Player.GetComponent<IDamageable>();    
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _player.TakeDamage(_dmg);
    }
}
