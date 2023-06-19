using System;
using System.Linq;
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
    public void SaveCurrentLevel()
    {
        var levelName = string.Empty;

        foreach (char c in SceneManager.GetActiveScene().name)
            if (char.IsNumber(c)) levelName += c;
        var currentLevel = Convert.ToInt32(levelName);

        bool lastLevel = currentLevel >= Helpers.TotalLevels;
        Helpers.PersistantData.persistantDataSaved.currentLevel = lastLevel || Helpers.PersistantData.persistantDataSaved.currentLevel > currentLevel ? Helpers.PersistantData.persistantDataSaved.currentLevel : currentLevel + 1;

        string nextScene = lastLevel && Helpers.PersistantData.persistantDataSaved.currentDeaths > ZonesManager.Instance.zones.Last().deathsNeeded ||
            ZonesManager.Instance.lastLevelsZone.Any(x => x == SceneManager.GetActiveScene().name) ? "LevelsMap" :
            lastLevel && Helpers.PersistantData.persistantDataSaved.currentDeaths <= ZonesManager.Instance.zones.Last().deathsNeeded ? "WinScreen"
            : $"Level {currentLevel + 1}";

        LoadLevel(nextScene);
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
        if (SceneManager.GetActiveScene().buildIndex == 40)
        {
            LoadLevel("WinScreen");
            return;
        }
        victoryTimeline.Play();
    }
}
