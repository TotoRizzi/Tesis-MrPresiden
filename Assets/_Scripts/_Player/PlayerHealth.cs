using System.Collections;
using System.Linq;
using UnityEngine;
using System.Diagnostics;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    GameManager _gameManager;

    [SerializeField] SpriteRenderer[] _allPlayerSprites;
    Vector3 _initialPos;

    private void Start()
    {
        _initialPos = transform.position;
        _gameManager = Helpers.GameManager;

        _gameManager.OnPlayerDead += Die;
    }

    public void TakeDamage(float dmg)
    {
        _gameManager.PlayerDead();
    }

    public void Die()
    {
        EffectsOnDeath();
        RestartPosition();
    }

    void EffectsOnDeath()
    {
        _gameManager.EffectsManager.HumanoindKilled(transform.position + Vector3.up);
        Helpers.AudioManager.PlaySFX("Enemy_Dead");

        _gameManager.Player.PausePlayer();
        foreach (var item in _allPlayerSprites)            
            item.gameObject.SetActive(false);
    }

    void RestartPosition()
    {
        transform.position = _initialPos;
        _gameManager.Player.UnPausePlayer();

        foreach (var item in _allPlayerSprites)
            item.gameObject.SetActive(true);
    }
}
