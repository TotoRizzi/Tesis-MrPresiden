using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    GameManager _gameManager;

    [SerializeField] float _maxHp;
    float _currentHp;

    private void Start()
    {
        _gameManager = GameManager.instance;

        _currentHp = _gameManager.SaveDataManager.GetFloat("CurrentHp", _maxHp);
    }

    public void TakeDamage(float dmg)
    {
        _currentHp -= dmg;
        _gameManager.SaveDataManager.SaveFloat("CurrentHp", _currentHp);
        if (_currentHp < 0) Die();
    }

    public void Die()
    {
        _gameManager.GameLost();
    }
}
