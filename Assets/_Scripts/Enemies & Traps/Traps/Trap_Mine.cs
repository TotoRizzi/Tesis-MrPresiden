using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Mine : Enemy
{
    [SerializeField] GameObject _damageOnTriggerGO;
    [SerializeField] GameObject _parentGO;
    private bool _isInCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isInCoroutine) Explode();

    }

    void Explode()
    {
        _isInCoroutine = true;
        _damageOnTriggerGO.SetActive(true);
        StartCoroutine(DestroyMine());
    }
    public override void Die()
    {
        gameManager.EnemyManager.RemoveEnemy(this);
        gameManager.EffectsManager.HumanoindKilled(transform.position);

        Destroy(_parentGO);
    }

    IEnumerator DestroyMine()
    {
        yield return new WaitForEndOfFrame();
        Die();
    }
}
