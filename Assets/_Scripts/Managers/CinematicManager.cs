using UnityEngine;
using UnityEngine.Playables;
public class CinematicManager : MonoBehaviour
{
    PlayableDirector _defeatTimeline, _victoryTimeline;
    [SerializeField, Tooltip("Está como hijo de la Main Camera")] GameObject _cinemacticCamera;
    void Start()
    {
        _cinemacticCamera.SetActive(false);
        _defeatTimeline = GameObject.Find("DefeatTimeline").GetComponent<PlayableDirector>();
        _victoryTimeline = GameObject.Find("VictoryTimeline").GetComponent<PlayableDirector>();
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
}
