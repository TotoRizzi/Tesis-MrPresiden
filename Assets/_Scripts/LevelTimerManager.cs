using System.Collections;
using UnityEngine;
public class LevelTimerManager : MonoBehaviour
{
    [SerializeField] float _timer;
    [SerializeField] float _levelMaxTime;
    [SerializeField] float _timeToDiscount;
    public float Timer { get { return _timer; } set { _timer = value; } }
    public float LevelMaxTime { get { return _levelMaxTime; } }

    bool _stopTimer;

    public System.Action RedButton; //Lo llamo en el NextSceneOnTrigger
    void Start()
    {
        Helpers.GameManager.EnemyManager.OnEnemyKilled += () => EarnTime(_timeToDiscount);
        RedButton += StopTimer;
        StartCoroutine(LevelTimer());
    }
    private void OnDisable()
    {
        RedButton -= StopTimer;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) RedButton();
    }
    IEnumerator LevelTimer()
    {
        while (_timer <= _levelMaxTime)
        {
            if (Helpers.GameManager.PauseManager.Paused) yield return null;
            else
            {
                if (_stopTimer) yield break;
                _timer += Time.deltaTime;
                yield return null;
            }
        }
        Helpers.GameManager.CinematicManager.PlayDefeatCinematic();
        Helpers.GameManager.PauseManager.PauseObjectsInCinematic();
    }
    public void StopTimer()
    {
        _stopTimer = true;
        Helpers.GameManager.EnemyManager.OnEnemyKilled -= () => EarnTime(_timeToDiscount);
    }
    public void EarnTime(float time)
    {
        _timer -= time;
    }
}
