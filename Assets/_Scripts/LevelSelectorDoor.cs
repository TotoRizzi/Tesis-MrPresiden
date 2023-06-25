using UnityEngine;
using TMPro;
using System.Linq;
public class LevelSelectorDoor : MonoBehaviour
{
    [SerializeField] int _levelIndex;
    [SerializeField] TextMeshProUGUI _levelIndexTxt, _deathAmountInLevel;

    Animator _anim;
    Collider2D _colliderNoTrigger;
    private void Start()
    {
        var index = Helpers.PersistantData.gameData.levels.Any(x => x == $"Level {_levelIndex}") ? Helpers.PersistantData.gameData.levels.IndexOf($"Level {_levelIndex}") : 0;
        var deathAmount = Helpers.PersistantData.gameData.deaths.Any() ? Helpers.PersistantData.gameData.deaths[index] : 0;
        _deathAmountInLevel.text = deathAmount.ToString();

        if (_levelIndex > Helpers.PersistantData.gameData.currentLevel) return;
        _anim = GetComponentInChildren<Animator>();
        _colliderNoTrigger = GetComponents<Collider2D>().Where(x => !x.isTrigger).FirstOrDefault();

        _levelIndexTxt.text = _levelIndex.ToString();


        ShowExit();
    }

    void ShowExit()
    {
        _anim.SetBool("IsOpen", true);
        _colliderNoTrigger.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            Helpers.GameManager.LoadSceneManager.LoadLevel("Level " + _levelIndex);
            Helpers.GameManager.Player.PausePlayer();
        }
    }
}
