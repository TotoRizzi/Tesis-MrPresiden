using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesDoor : MonoBehaviour
{
    [SerializeField] Animator _anim;
    Collider2D _collider;
    WavesEnemyManager _enemyManager;

    [SerializeField] List<int> _allLevels;
    int[] _newLevelOrder;

    private void Start()
    {
        _enemyManager = FindObjectOfType<WavesEnemyManager>();
        if (!_enemyManager) Debug.LogError("NO WavesEnemyManager");

        _enemyManager.OnWaveWon += ShowExit;

        _collider = GetComponent<Collider2D>();

        StartCoroutine(HideExit());
        StartCoroutine(NewLevels());
    }

    IEnumerator NewLevels()
    {
        yield return new WaitForEndOfFrame();
        if (Helpers.GameManager.SaveDataManager.GetBool("LevelsSetted", false))
            GetNewLevelOrder();
        else
            SetNewOrderOfLevels();

    }

    void ShowExit()
    {
        _anim.SetBool("IsOpen", true);
        _collider.enabled = true;
    }
    IEnumerator HideExit()
    {
        yield return new WaitForEndOfFrame();
        _collider.enabled = false;
        _anim.SetBool("IsOpen", false);

    }
    void SetNewOrderOfLevels()
    {
        int allLevels = _allLevels.Count;
        _newLevelOrder = new int[allLevels];

        for (int i = 0; i < allLevels; i++)
        {
            int _randomLevel = Random.Range(0, _allLevels.Count);

            _newLevelOrder[i] = _allLevels[_randomLevel];

            _allLevels.Remove(_allLevels[_randomLevel]);

            Helpers.GameManager.SaveDataManager.SaveInt(i + 1.ToString(), _newLevelOrder[i]);
        }
        _enemyManager.currentLevel = 0;
        Helpers.GameManager.SaveDataManager.SaveInt("CurrentLevel", _enemyManager.currentLevel);

        Helpers.GameManager.SaveDataManager.SaveBool("LevelsSetted", true);

        for (int i = 0; i < _newLevelOrder.Length; i++)
        {
            Debug.Log(Helpers.GameManager.SaveDataManager.GetInt(i + 1.ToString(), 0));
        }

        //GameManager.instance.SaveData();
    }

    void GetNewLevelOrder()
    {
        int allLevels = _allLevels.Count;
        _newLevelOrder = new int[allLevels];

        _enemyManager.currentLevel = Helpers.GameManager.SaveDataManager.GetInt("CurrentLevel", 0);
        _enemyManager.currentLevel++;
        Helpers.GameManager.SaveDataManager.SaveInt("CurrentLevel", _enemyManager.currentLevel);

        for (int i = 0; i < allLevels; i++)
        {
            _newLevelOrder[i] = Helpers.GameManager.SaveDataManager.GetInt(i + 1.ToString(), 0);
        }
    }

    public void NextLevel()
    {
        int fixedCurrentLevel = _enemyManager.currentLevel;

        if (fixedCurrentLevel < _newLevelOrder.Length)
        {
            int levels = PlayerPrefs.GetInt("LevelsWinned") + 1;
            PlayerPrefs.SetInt("LevelsWinned", levels);
            Helpers.GameManager.LoadSceneManager.LoadLevel("MiniGame 1 " + _newLevelOrder[_enemyManager.currentLevel]);
        }
        else
            EventManager.TriggerEvent(Contains.WIN_WAVESGAME); //CUANDO TERMINA TODOS LOS NIVELES EJECUTAMOS ESTE EVENTO
    }

    private void OnTriggerEnter2D()
    {
        if (Helpers.GameManager.UiManager == null) return;

        NextLevel();
        if (Helpers.GameManager.Player) Helpers.GameManager.Player.PausePlayer();
    }
}
