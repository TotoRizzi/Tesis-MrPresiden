using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    GameManager _gameManager;
    [SerializeField] int _currentLevel;
    [SerializeField] int _numberOfLevels;
    int[] _newLevelOrder;

    private void Start()
    {
        _gameManager = GameManager.instance;

        _currentLevel = _gameManager.SaveDataManager.GetInt("CurrentLevel", 0);

        bool isSaved = _gameManager.SaveDataManager.GetBool("HasSavedLevel", false);
        Debug.Log(isSaved);
        if (isSaved)
            _newLevelOrder = _gameManager.SaveDataManager.Getrray("Level ", _numberOfLevels);
        else
            SetNewOrderOfLevels();
    }
    void SetNewOrderOfLevels()
    {
        _newLevelOrder = new int[_numberOfLevels];

        var firstList = new List<int>();

        for (int i = 1; i <= _numberOfLevels; i++)
        {
            firstList.Add(i);
        }

        for (int i = 0; i < _numberOfLevels; i++)
        {
            int _randomLevel = Random.Range(0, firstList.Count - 1);

            _newLevelOrder[i] = firstList[_randomLevel];

            firstList.Remove(firstList[_randomLevel]);
        }
        _currentLevel = 0;

        _gameManager.SaveDataManager.SaveLevels("Level ", _newLevelOrder);
        _gameManager.SaveDataManager.SaveBool("HasSavedLevel", true);

    }

    public void NextLevel()
    {
        int fixedCurrentLevel = _currentLevel;
        _currentLevel++;

        if (fixedCurrentLevel < _newLevelOrder.Length)
        {
            _gameManager.SaveDataManager.SaveInt("CurrentLevel", _currentLevel);

            _gameManager.SceneManager.LoadLevel(fixedCurrentLevel);

            Debug.Log((_newLevelOrder[fixedCurrentLevel]));
        }
    }
}