using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class LevelTimerManager : MonoBehaviour
{
    [SerializeField] float _timer;
    [SerializeField] float _levelMaxTime;
    [SerializeField] float _timeToDiscount;
    [SerializeField, Tooltip("Está como hijo de la Main Camera")] GameObject _cinematicCamera;
    public float Timer { get { return _timer; } set { _timer = value; } }
    public float LevelMaxTime { get { return _levelMaxTime; } }

    PlayableDirector _timeline;
    bool _stopTimer;

    public event System.Action RedButton;
    void Start()
    {
        _cinematicCamera.SetActive(false);
        _timeline = GameObject.Find("Timeline").GetComponent<PlayableDirector>();
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
        _cinematicCamera.SetActive(true);
        _timeline.Play();
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
