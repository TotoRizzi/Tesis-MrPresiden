using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    GameManager _gameManager;
    private void Start()
    {
        _gameManager = GameManager.instance;

        _gameManager.OnGameLost += GameLost;
        _gameManager.OnSpiked += RestartLevel;
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene("Level " + level);
    }

    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    void GameLost()
    {
        LoadLevel("Menu");
    }
    void RestartLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }
}
