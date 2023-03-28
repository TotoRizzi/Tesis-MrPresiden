using System.Collections;
using UnityEngine;
public class LevelTimerManager : MonoBehaviour
{
    [SerializeField] float _timer;
    [SerializeField] float _levelMaxTime;
    [SerializeField] GameObject _cinematicCamera;
    public float Timer { get { return _timer; } set { _timer = value; } }
    public float LevelMaxTime { get { return _levelMaxTime; } }
    void Start()
    {
        Helpers.GameManager.EnemyManager.OnEnemyKilled += () => EarnTime(1);
        StartCoroutine(LevelTimer());
    }
    IEnumerator LevelTimer()
    {
        while (_timer <= _levelMaxTime)
        {
            _timer += Time.deltaTime;
            yield return null;
        }
        _cinematicCamera.SetActive(true);

    }
    public void EarnTime(float time)
    {
        _timer -= time;
    }
}
