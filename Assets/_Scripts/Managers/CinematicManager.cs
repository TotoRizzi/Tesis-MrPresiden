using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
public class CinematicManager : MonoBehaviour
{
    PlayableDirector _defeatTimeline, _victoryTimeline, _initialTimeline;
    [SerializeField, Tooltip("Está como hijo de la Main Camera")] GameObject _cinemacticCamera;
    void Start()
    {
        _cinemacticCamera.SetActive(false);
        _initialTimeline = GameObject.Find("InitialTimeline").GetComponent<PlayableDirector>();
        _defeatTimeline = GameObject.Find("DefeatTimeline").GetComponent<PlayableDirector>();
        _victoryTimeline = GameObject.Find("VictoryTimeline").GetComponent<PlayableDirector>();
        var currentScene = SceneManager.GetActiveScene().ToString();
        if (!PlayerPrefs.HasKey(currentScene)) _initialTimeline.Play();
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
