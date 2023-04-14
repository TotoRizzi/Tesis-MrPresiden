using System.Collections;
using UnityEngine;
using System;
public class LevelTimerManager : MonoBehaviour
{
    [SerializeField] float _timer;
    [SerializeField] float _levelMaxTime;
    [SerializeField] float _timeToDiscount;
    [SerializeField] Animator _levelLightsAnimator;
    public float Timer { get { return _timer; } set { _timer = value; } }
    public float LevelMaxTime { get { return _levelMaxTime; } }

    bool _stopTimer;
    bool _stopTrap;
    bool _firstTime;

    public Action RedButton;
    void Start()
    {
        Helpers.GameManager.EnemyManager.OnEnemyKilled += StopTrap;
        RedButton += WinLevel;
    }
    private void OnDisable()
    {
        RedButton -= WinLevel;
    }
    public void StartLevelTimer() { StartCoroutine(LevelTimer()); }
    IEnumerator LevelTimer()
    {
        if (_firstTime) yield break;
        _levelLightsAnimator.Play("LevelLight_Open");
        _firstTime = true;
        WaitForSeconds wait = new WaitForSeconds(_timeToDiscount);
        while (_timer <= _levelMaxTime)
        {
            if (Helpers.GameManager.PauseManager.Paused) yield return null;
            else
            {
                if (_stopTimer) yield break;                            //Cuando pongo pausa
                if (_stopTrap)
                {
                    yield return wait;                                  //Cuando muere un enemigo
                    _levelLightsAnimator.SetBool("_stopLights", false);
                }
                _stopTrap = false;
                _timer += Time.deltaTime;
                yield return null;
            }
        }
        _levelLightsAnimator.SetBool("_stopLights", true);
        Helpers.GameManager.CinematicManager.PlayDefeatCinematic();
        Helpers.GameManager.PauseManager.PauseObjectsInCinematic();
    }
    public void WinLevel()
    {
        _stopTimer = true;
        Helpers.GameManager.EnemyManager.OnEnemyKilled -= StopTrap;
    }
    void StopTrap()
    {
        _levelLightsAnimator.SetBool("_stopLights", true);
        _stopTrap = true;
    }
}
