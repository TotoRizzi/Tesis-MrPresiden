using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    Animator _anim;
    WaitForSeconds _wait = new WaitForSeconds(.9f);
    private void Start()
    {
        _anim = GetComponent<Animator>();

        if (Helpers.GameManager) Helpers.GameManager.OnGameLost += () => LoadLevel("Menu");
        if (Helpers.GameManager) Helpers.GameManager.OnGameWon += () => LoadLevel("WinScreen");
        //Helpers.GameManager.OnSpiked += () => LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }
    public void ReloadLevel() => StartCoroutine(ChangeScene(SceneManager.GetActiveScene().buildIndex));
    public void LoadLevel(int levelIndex) => StartCoroutine(ChangeScene(levelIndex));
    public void LoadLevel(string levelName) => StartCoroutine(ChangeScene(levelName));
    public void UnlockNewZone() => Helpers.PersistantData.persistantDataSaved.unbloquedZones++;
    public void SaveCurrentLevel()
    {
        var levelName = string.Empty;

        foreach (char c in SceneManager.GetActiveScene().name)
            if (char.IsNumber(c)) levelName += c;
        var currentLevel = Convert.ToInt32(levelName);
        Helpers.PersistantData.persistantDataSaved.currentLevel = Convert.ToInt32(currentLevel + 1);
    }
    IEnumerator ChangeScene(int levelIndex = default)
    {
        _anim.Play("Close");
        yield return _wait;
        SceneManager.LoadScene(levelIndex);
    }
    IEnumerator ChangeScene(string levelName)
    {
        _anim.Play("Close");
        yield return _wait;
        SceneManager.LoadScene(levelName);
    }

    public void NextSceneFast(UnityEngine.Playables.PlayableDirector victoryTimeline)
    {
        if (SceneManager.GetActiveScene().name == "WinScreen" || SceneManager.GetActiveScene().name == "Menu") return;
        if (SceneManager.GetActiveScene().buildIndex == 36)
        {
            LoadLevel("WinScreen");
            return;
        }
        victoryTimeline.Play();
    }
}
