using UnityEngine;
public class MenuManager : MonoBehaviour
{
    GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.instance;
    }

    public void BTN_Play()
    {
        _gameManager.SaveDataManager.Reset();
        _gameManager.LevelManager.SetNewOrderOfLevels();
        _gameManager.LevelManager.NextLevel();
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
        _gameManager.SceneManager.GoToMenu();

    }
    public void BTN_Tutorial()
    {
        _gameManager.SceneManager.LoadLevel("Tutorial1");
        _gameManager.SaveDataManager.Reset();
    }
}
