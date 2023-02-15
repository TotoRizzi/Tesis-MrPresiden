using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    GameManager _gameManager;

    [SerializeField] Renderer[] _allPlayerSprites;
    [SerializeField] float _defaultHp;
    float _maxHp;
    float _currentHp;

    [SerializeField] float _invincibilityTime = .1f;
    bool _invincible = false;

    private void Start()
    {
        _gameManager = GameManager.instance;
        _gameManager.OnAchievementReached += AddHealth;
        _gameManager.OnSpiked += EffectsOnDeath;

        _maxHp = _gameManager.SaveDataManager.GetFloat("MaxHp", _defaultHp);
        _currentHp = _gameManager.SaveDataManager.GetFloat("CurrentHp", _maxHp);
        UpdateUi();
    }

    public void TakeDamage(float dmg)
    {
        UnityEngine.Debug.Log(new StackTrace().GetFrame(0).GetMethod().Name);

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
        EffectsOnDeath();
        StartCoroutine(MenuDelay());
    }

    void EffectsOnDeath()
    {
        for (int i = 0; i < 3; i++)
        {
            _gameManager.EffectsManager.HumanoindKilled(transform.position + Vector3.up);
            _gameManager.SoundManager.PlaySound("Enemy_Dead");
        }

        Destroy(gameObject);
    }

    IEnumerator MenuDelay()
    {
        yield return new WaitForSeconds(.5f);
        _gameManager.GameLost();
    }

    void AddHealth()
    {
        _currentHp++;
        if (_currentHp > _maxHp)
            _maxHp++;

        SaveData();
        UpdateUi();
    }

    void SaveData()
    {
        _gameManager.SaveDataManager.SaveFloat("CurrentHp", _currentHp);
        _gameManager.SaveDataManager.SaveFloat("MaxHp", _maxHp);
    }
    void UpdateUi()
    {
        _gameManager.UiManager.UpdateHealthBar(_currentHp, _maxHp);
    }
}
