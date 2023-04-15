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
}
