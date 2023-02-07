using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Mine : Enemy
{
    [SerializeField] GameObject _damageOnTriggerGO;
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

    IEnumerator DestroyMine()
    {
        yield return new WaitForEndOfFrame();
        Die();
    }
}
