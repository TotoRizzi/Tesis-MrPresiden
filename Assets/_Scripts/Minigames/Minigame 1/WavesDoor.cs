using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesDoor : MonoBehaviour
{
    [SerializeField] string _nextScene;
    [SerializeField] Animator _anim;
    Collider2D _collider;
    WavesEnemyManager _enemyManager;
    
    private void Start()
    {
        _enemyManager = FindObjectOfType<WavesEnemyManager>();
        if (!_enemyManager) Debug.LogError("NO WavesEnemyManager");

        _enemyManager.OnWaveWon += ShowExit;

        _collider = GetComponent<Collider2D>();
        StartCoroutine(HideExit());
    }
    void ShowExit()
    {
        _anim.SetBool("IsOpen", true);
        _collider.enabled = true;
    }
    IEnumerator HideExit()
    {
        yield return new WaitForEndOfFrame();
        _collider.enabled = false;
        _anim.SetBool("IsOpen", false);

    }
    private void OnTriggerEnter2D()
    {
        if (Helpers.GameManager.UiManager == null) return;

        Helpers.GameManager.LoadSceneManager.LoadLevel(_nextScene);
        if (Helpers.GameManager.Player) Helpers.GameManager.Player.PausePlayer();
    }
}
