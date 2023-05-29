using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Malo : MonoBehaviour
{
    Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
        Helpers.GameManager.EnemyManager.OnEnemyKilled += () => _animator.Play("Susto");
        Helpers.LevelTimerManager.OnLevelDefeat += () => _animator.Play("MaloWon");
    }
}
