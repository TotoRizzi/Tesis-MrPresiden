using UnityEngine;
using UnityEngine.Playables;

public class MenuManager : MonoBehaviour
{
    [SerializeField] PlayableDirector _trainCinematic;
    public void BTN_Play()
    {
        var gameMode = Helpers.GameManager.SaveDataManager.GetInt(Helpers.GameManager.gameModeSaveName, (int)GameMode.EasyGameMode);
        bool trainCinematic = PlayerPrefs.HasKey("TrainCinematic");

        PlayerPrefs.DeleteAll();
        Helpers.GameManager.SaveDataManager.SaveInt(Helpers.GameManager.gameModeSaveName, gameMode);

        //if (trainCinematic) Helpers.GameManager.LoadSceneManager.LoadLevel("Level 0.1");
        //else
        //{
        //    _trainCinematic.Play();
        //    PlayerPrefs.SetString("TrainCinematic", "TrainCinematic");
        //}
        _trainCinematic.Play();
    }
    public void BTN_Credits()
    {

    }
    public void BTN_Quit()
    {
        Application.Quit();
    }

    public void BTN_GoToMenu()
    {
        Helpers.GameManager.LoadSceneManager.LoadLevel("Menu");
    }
    public void BTN_Tutorial()
    {
        Helpers.GameManager.LoadSceneManager.LoadLevel("Tutorial1");
        Helpers.GameManager.SaveDataManager.Reset();
    }
}
