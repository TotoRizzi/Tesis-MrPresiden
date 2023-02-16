using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Scene_Manager : MonoBehaviour
{
    GameManager _gameManager;
    private void Start()
    {
        _gameManager = GameManager.instance;

        _gameManager.OnGameLost += GameLost;
        _gameManager.OnSpiked += ()  => StartCoroutine(RestartLevel());
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.T)) RestartLevel();
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
    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(1f);
        LoadLevel(SceneManager.GetActiveScene().name);
    }
    
    public void GoToMenu()
    {
        LoadLevel("Menu");
    }
}
