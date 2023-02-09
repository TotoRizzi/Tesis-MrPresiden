using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.OnGameLost += GameLost;
    }
    public void GoToNextScene()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex;
        nextScene++;

        SceneManager.LoadScene(nextScene);
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene("Level " + level);
    }
    void GameLost()
    {
        //Reinicia el nivel, en el futuro, que mande a la pantalla de derrota
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
