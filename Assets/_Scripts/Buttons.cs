using UnityEngine;
public class Buttons : MonoBehaviour
{
    public void GoToMenu()
    {
        Helpers.GameManager.LoadSceneManager.LoadLevel("Menu");
    }
    public void Retry()
    {
        Helpers.GameManager.LoadSceneManager.ReloadLevel();
        PlayerPrefs.DeleteAll();
    }
    public void LoadScene(string sceneName)
    {
        Helpers.GameManager.LoadSceneManager.LoadLevel(sceneName);
    }
    public void LoadScene(int sceneIndex)
    {
        Helpers.GameManager.LoadSceneManager.LoadLevel(sceneIndex);
    }
}
