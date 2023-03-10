using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Disappear : MonoBehaviour
{
    [SerializeField] float _timeToDisappear;
    [SerializeField] float _timeToAppear;

    [SerializeField] GameObject _sprite;
    Collider2D[] _colliders;
    Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _colliders = GetComponents<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")) StartCoroutine(Disappear());
    }
    IEnumerator Disappear()
    {
        _anim.Play("Breaking");
        yield return new WaitForSeconds(_timeToDisappear);
        _sprite.SetActive(false);

        foreach (var item in _colliders)
        {
            item.enabled = false;
        }

        StartCoroutine(Appear());
    }
    IEnumerator Appear()
    {
        yield return new WaitForSeconds(_timeToAppear);

        _sprite.SetActive(true);

        foreach (var item in _colliders)
        {
            item.enabled = true;
        }
        _anim.Play("Idle");

    }
}
