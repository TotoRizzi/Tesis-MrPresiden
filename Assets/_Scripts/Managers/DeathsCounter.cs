using UnityEngine;
using UnityEngine.SceneManagement;
public class DeathsCounter : MonoBehaviour
{
    void Start()
    {
        var levelName = SceneManager.GetActiveScene().name;

        if (Helpers.PersistantData.persistantDataSaved.levels.Contains(levelName)) Helpers.PersistantData.persistantDataSaved.deaths[Helpers.PersistantData.persistantDataSaved.levels.IndexOf(levelName)] = 0;
        else
        {
            Helpers.PersistantData.persistantDataSaved.levels.Add(levelName);
            Helpers.PersistantData.persistantDataSaved.deaths.Add(0);
        }

        Helpers.GameManager.OnPlayerDead += () => Helpers.PersistantData.persistantDataSaved.deaths[Helpers.PersistantData.persistantDataSaved.levels.IndexOf(levelName)]++;
    }
}
