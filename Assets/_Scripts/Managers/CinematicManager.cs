using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
public class CinematicManager : MonoBehaviour
{
    PlayableDirector _defeatTimeline, _victoryTimeline, _initialTimeline;
    [SerializeField, Tooltip("Está como hijo de la Main Camera")] GameObject _cinemacticCamera;

    public bool playingCinematic
    {
        get { return _initialTimeline && _victoryTimeline && _defeatTimeline ? _initialTimeline.state == PlayState.Playing || _victoryTimeline.state == PlayState.Playing || _defeatTimeline.state == PlayState.Playing : default; }
    }
    void Start()
    {
        Helpers.LevelTimerManager.OnLevelDefeat += PlayDefeatCinematic;

        _cinemacticCamera.SetActive(false);
        _initialTimeline = GameObject.Find("InitialTimeline").GetComponent<PlayableDirector>();
        _defeatTimeline = GameObject.Find("DefeatTimeline").GetComponent<PlayableDirector>();
        _victoryTimeline = GameObject.Find("VictoryTimeline").GetComponent<PlayableDirector>();
        var currentScene = "Timeline " + SceneManager.GetActiveScene().name;
        if (!PlayerPrefs.HasKey(currentScene))
        {
            _initialTimeline.Play();
            Helpers.GameManager.PauseManager.PauseObjectsInCinematic();
            _initialTimeline.stopped += (x) => Helpers.GameManager.PauseManager.UnpauseObjectsInCinematic();
        }
        PlayerPrefs.SetString(currentScene, currentScene);
    }
    public void PlayDefeatCinematic()
    {
        _cinemacticCamera.SetActive(true);
        _defeatTimeline.Play();
    }
    public void PlayVictoryCinematic()
    {
        _cinemacticCamera.SetActive(true);
        Helpers.GameManager.PauseManager.PauseObjectsInCinematic();
        _victoryTimeline.Play();
    }
    public void SetActiveCinematicCamera(bool enabled)
    {
        _cinemacticCamera.SetActive(enabled);
    }
    public void LerpSize(float a, float b, float time)
    {
        _cinemacticCamera.GetComponent<Camera>().orthographicSize = Mathf.Lerp(a, b, time);
    }
}
