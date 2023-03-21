using UnityEngine;
public class MenuManager : MonoBehaviour
{
    public void BTN_Play()
    {
        PlayerPrefs.DeleteAll();
        //_gameManager.LevelManager.SetNewOrderOfLevels();
        //_gameManager.LevelManager.NextLevel();
        Helpers.GameManager.LoadSceneManager.LoadLevel("level 1");
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
