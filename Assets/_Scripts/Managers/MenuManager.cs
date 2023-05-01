using UnityEngine;
public class MenuManager : MonoBehaviour
{
    public void BTN_Play()
    {
        var gameMode = Helpers.GameManager.SaveDataManager.GetInt(Helpers.GameManager.gameModeSaveName, (int)GameMode.EasyGameMode);
        PlayerPrefs.DeleteAll();
        Helpers.GameManager.SaveDataManager.SaveInt(Helpers.GameManager.gameModeSaveName, gameMode);
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
