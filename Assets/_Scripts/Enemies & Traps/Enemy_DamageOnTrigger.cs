using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DamageOnTrigger : MonoBehaviour
{
    IDamageable _player;
    [SerializeField] float _dmg;

    Collider2D _collider;

    private void Start()
    {
        _player = GameManager.instance.Player.GetComponent<IDamageable>();
        _collider = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        _player.TakeDamage(_dmg);
        Debug.Log("Damageando al player" + gameObject.GetComponentInParent<Transform>().name);
        StartCoroutine(ActivateAndDesactivateColldier());
    }
    IEnumerator ActivateAndDesactivateColldier()
    {
        _collider.enabled = false;
        yield return new WaitForSeconds(1f);
        _collider.enabled = true;
    }
}
