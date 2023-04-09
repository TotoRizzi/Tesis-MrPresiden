using System.Collections;
using UnityEngine;
using System;
public class LevelTimerManager : MonoBehaviour
{
    [SerializeField] float _timer;
    [SerializeField] float _levelMaxTime;
    [SerializeField] float _timeToDiscount;
    [SerializeField] bool _stopTrapMode;
    public float Timer { get { return _timer; } set { _timer = value; } }
    public float LevelMaxTime { get { return _levelMaxTime; } }

    bool _stopTimer;
    bool _stopTrap;

    public Action RedButton;
    void Start()
    {
        Helpers.GameManager.EnemyManager.OnEnemyKilled += _stopTrapMode ? (Action)StopTrap : (Action)GoBackTrap;
        RedButton += WinLevel;
        StartCoroutine(LevelTimer());
    }
    private void OnDisable()
    {
        RedButton -= WinLevel;
    }
    IEnumerator LevelTimer()
    {
        WaitForSeconds wait = new WaitForSeconds(_timeToDiscount);
        while (_timer <= _levelMaxTime)
        {
            if (Helpers.GameManager.PauseManager.Paused) yield return null;
            else
            {
                if (_stopTimer) yield break;
                if (_stopTrap) yield return wait;
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
        Helpers.GameManager.EnemyManager.OnEnemyKilled -= GoBackTrap;
    }

    void GoBackTrap()
    {
        EarnTime(_timeToDiscount);
    }
    void EarnTime(float time)
    {
        _timer -= time;
    }
    void StopTrap()
    {
        _stopTrap = true;
    }
}
