using System.Collections;
using UnityEngine;
public class LevelTimerManager : MonoBehaviour
{
    [SerializeField] float _timer;
    [SerializeField] float _levelMaxTime;
    [SerializeField] GameObject _cinematicCamera;
    public float Timer { get { return _timer; } set { _timer = value; } }
    public float LevelMaxTime { get { return _levelMaxTime; } }

    IEnumerator _timerCoroutime = null;
    void Start()
    {
        Helpers.GameManager.EnemyManager.OnEnemyKilled += () => EarnTime(1);
        _timerCoroutime = LevelTimer();
        StartCoroutine(LevelTimer());
    }
    public void StopTimer() => Time.timeScale = 0;
    public void StartTimer() => Time.timeScale = 1;
    IEnumerator LevelTimer()
    {
        while (_timer <= _levelMaxTime)
        {
            if (Helpers.GameManager.PauseManager.Paused) yield return null;
            else
            {
                _timer += Time.deltaTime;
                yield return null;
            }
        }
        _cinematicCamera.SetActive(true);
    }
    public void EarnTime(float time)
    {
        _timer -= time;
    }
}
