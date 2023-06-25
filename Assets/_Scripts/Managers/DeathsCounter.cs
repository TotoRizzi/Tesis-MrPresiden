using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
public class DeathsCounter : MonoBehaviour
{
    void Start()
    {
        GameData persistantDataSaved = Helpers.PersistantData.gameData;

        var levelName = SceneManager.GetActiveScene().name;

        if (!persistantDataSaved.levels.Contains(levelName))
        {
            persistantDataSaved.levels.Add(levelName);
            persistantDataSaved.deaths.Add(0);
        }

        int deaths = 0;
        Helpers.GameManager.OnPlayerDead += () => deaths++;

        Helpers.LevelTimerManager.RedButton += () =>
        {
            persistantDataSaved.deaths[persistantDataSaved.levels.IndexOf(levelName)] = deaths;
            persistantDataSaved.currentDeaths = persistantDataSaved.deaths.Any() ? persistantDataSaved.deaths.Sum() : default;
        };
        Helpers.LevelTimerManager.OnLevelDefeat += () => persistantDataSaved.deaths[persistantDataSaved.levels.IndexOf(levelName)] = deaths;
    }
}
