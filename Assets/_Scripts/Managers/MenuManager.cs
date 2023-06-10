using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
public class MenuManager : MonoBehaviour
{
    [SerializeField] PlayableDirector _trainCinematic;
    [SerializeField] Button _continueButton;
    private void Start()
    {
        _continueButton.gameObject.SetActive(!Helpers.PersistantData.persistantDataSaved.firstTime);
    }
    public void BTN_NewGame()
    {
        Helpers.PersistantData.DeletePersistantData();
        PlayerPrefs.DeleteAll();
        _trainCinematic.Play();
        Helpers.PersistantData.persistantDataSaved.firstTime = false;
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
