using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using System.Collections;
public class CinematicManager : MonoBehaviour
{
    PlayableDirector _defeatTimeline, _victoryTimeline, _initialTimeline;
    [SerializeField, Tooltip("Est� como hijo de la Main Camera")] GameObject _cinemacticCamera;
    [SerializeField] GameObject _skipCinematicUI;

    public bool playingCinematic
    {
        get { return _initialTimeline && _victoryTimeline && _defeatTimeline ? _initialTimeline.state == PlayState.Playing || _victoryTimeline.state == PlayState.Playing || _defeatTimeline.state == PlayState.Playing : default; }
    }
    void Start()
    {
        Helpers.LevelTimerManager.OnLevelDefeat += PlayDefeatCinematic;

        _cinemacticCamera.SetActive(false);
        _initialTimeline = GameObject.Find("InitialTimeline") ? GameObject.Find("InitialTimeline").GetComponent<PlayableDirector>() : null;
        _defeatTimeline = GameObject.Find("DefeatTimeline") ? GameObject.Find("DefeatTimeline").GetComponent<PlayableDirector>(): null;
        _victoryTimeline = GameObject.Find("VictoryTimeline") ? GameObject.Find("VictoryTimeline").GetComponent<PlayableDirector>() : null ;
        var currentScene = "initialTimeline " + SceneManager.GetActiveScene().name;

        if (!PlayerPrefs.HasKey(currentScene) && _initialTimeline)
        {
            Helpers.MainCamera.GetComponent<CameraAnimation>().enabled = true;
            _initialTimeline.Play();
            Helpers.GameManager.PauseManager.PauseObjectsInCinematic();
            _initialTimeline.stopped += (x) => Helpers.GameManager.PauseManager.UnpauseObjectsInCinematic();
        }
        PlayerPrefs.SetString(currentScene, currentScene);
    }
    public void PlayDefeatCinematic()
    {
        if (!_defeatTimeline) Helpers.GameManager.LoadSceneManager.ReloadLevel();

        var defeatCinematic = "defeatTimeline " + SceneManager.GetActiveScene().name;
        _cinemacticCamera.SetActive(true);
        _defeatTimeline.Play();

        if (PlayerPrefs.HasKey(defeatCinematic)) StartCoroutine(ShowSkipUI());

        PlayerPrefs.SetString(defeatCinematic, defeatCinematic);
    }

    IEnumerator ShowSkipUI()
    {
        yield return new WaitForSeconds(1);
        _skipCinematicUI.SetActive(true);
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
    public void SetSizeCinematicCamera(float size)
    {
        _cinemacticCamera.GetComponent<Camera>().orthographicSize = size;
    }
    public void SkipDefeatCinematic()
    {
        _defeatTimeline.Stop();
        Helpers.GameManager.LoadSceneManager.ReloadLevel();
    }
}
