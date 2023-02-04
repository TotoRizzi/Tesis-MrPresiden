using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    public void GoToNextScene()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex;
        nextScene++;

        SceneManager.LoadScene(nextScene);
    }
}
