using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    Animator _anim;
    WaitForSeconds _wait = new WaitForSeconds(.9f);
    private void Start()
    {
        _anim = GetComponent<Animator>();

        Helpers.GameManager.OnGameLost += () => LoadLevel("Menu");
        Helpers.GameManager.OnGameWon += () => LoadLevel("WinScreen");
        //Helpers.GameManager.OnSpiked += () => LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLevel(int levelIndex) => StartCoroutine(ChangeScene(levelIndex));
    public void LoadLevel(string levelName) => StartCoroutine(ChangeScene(levelName));
    IEnumerator ChangeScene(int levelIndex)
    {
        _anim.Play("Close");
        yield return _wait;
        SceneManager.LoadScene(levelIndex);
    }
    IEnumerator ChangeScene(string levelName)
    {
        _anim.Play("Close");
        yield return _wait;
        SceneManager.LoadScene(levelName);
    }
}
