using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    GameManager _gameManager;

    [SerializeField] Renderer[] _allPlayerSprites;
    [SerializeField] float _maxHp;
    float _currentHp;

    [SerializeField] float _invincibilityTime = .1f;
    bool _invincible = false;

    private void Start()
    {
        _gameManager = GameManager.instance;

        _currentHp = _gameManager.SaveDataManager.GetFloat("CurrentHp", _maxHp);
        UpdateUi();
    }

    public void TakeDamage(float dmg)
    {
        if (_invincible) return;

        StartCoroutine(Invincible());

        _currentHp -= dmg;

        UpdateUi();
        SaveData();
        
        if (_currentHp <= 0) Die();
    }

    IEnumerator Invincible()
    {
        _invincible = true;

        foreach (var item in _allPlayerSprites)
        {
            item.material.color = Color.red;
        }

        yield return new WaitForSeconds(_invincibilityTime);

        _invincible = false;
        foreach (var item in _allPlayerSprites)
        {
            item.material.color = Color.white;
        }
    }
    public void Die()
    {
        _gameManager.GameLost();
    }
    void SaveData()
    {
        _gameManager.SaveDataManager.SaveFloat("CurrentHp", _currentHp);
    }
    void UpdateUi()
    {
        _gameManager.UiManager.UpdateHealthBar(_currentHp, _maxHp);
    }
}
