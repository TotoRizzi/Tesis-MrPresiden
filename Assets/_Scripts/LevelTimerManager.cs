using System.Collections;
using UnityEngine;
using System;
public class LevelTimerManager : MonoBehaviour
{
    [SerializeField] float _timer;
    [SerializeField] float _levelMaxTime;
    [SerializeField] float _timeToDiscount;
    public float Timer { get { return _timer; } set { _timer = value; } }
    public float LevelMaxTime { get { return _levelMaxTime; } }
    public float TimeToDiscount { get { return _timeToDiscount; } }

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
        _firstTime = true;
        WaitForSeconds wait = new WaitForSeconds(_timeToDiscount);
        while (_timer <= _levelMaxTime)
        {
            if (Helpers.GameManager.PauseManager.Paused) yield return null;
            else
            {
                if (_stopTimer) yield break;                            //Cuando pongo pausa
                if (_stopTrap) yield return wait;                                  //Cuando muere un enemigo
                _stopTrap = false;
                _timer += Time.deltaTime;
                yield return null;
            }
        }
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
        _stopTrap = true;
    }
}
