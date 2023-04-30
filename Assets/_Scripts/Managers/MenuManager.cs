using UnityEngine;
public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject _gameModeCanvas;
    [SerializeField] GameObject _playButton;

    public void BTN_ShowGameModeCanvas()
    {
        _gameModeCanvas.SetActive(true);
    }

    public void BTN_Easy()
    {
        PlayerPrefs.DeleteAll();

        Helpers.GameManager.SaveDataManager.SaveInt(Helpers.GameManager.gameModeSaveName, (int)GameMode.EasyGameMode);
        _playButton.SetActive(true);
    }

    public void BTN_Hard()
    {
        PlayerPrefs.DeleteAll();

        Helpers.GameManager.SaveDataManager.SaveInt(Helpers.GameManager.gameModeSaveName, (int)GameMode.HardGameMode);
        _playButton.SetActive(true);
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
