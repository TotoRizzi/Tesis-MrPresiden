using UnityEngine;
using UnityEngine.SceneManagement;
public class DeathsCounter : MonoBehaviour
{
    void Start()
    {
        var levelName = SceneManager.GetActiveScene().name;

        if (!Helpers.PersistantData.persistantDataSaved.levels.Contains(levelName))
        {
            Helpers.PersistantData.persistantDataSaved.levels.Add(levelName);
            Helpers.PersistantData.persistantDataSaved.deaths.Add(0);
        }

        int deaths = 0;
        Helpers.GameManager.OnPlayerDead += () => deaths++;

        Helpers.LevelTimerManager.RedButton += () => Helpers.PersistantData.persistantDataSaved.deaths[Helpers.PersistantData.persistantDataSaved.levels.IndexOf(levelName)] = deaths;
        Helpers.LevelTimerManager.OnLevelDefeat += () => Helpers.PersistantData.persistantDataSaved.deaths[Helpers.PersistantData.persistantDataSaved.levels.IndexOf(levelName)] = deaths;
    }
}
