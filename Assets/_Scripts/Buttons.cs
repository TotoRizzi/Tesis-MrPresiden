using UnityEngine;
public class Buttons : MonoBehaviour
{
    public void GoToMenu()
    {
        Helpers.GameManager.LoadSceneManager.LoadLevel("Menu");
    }
}
