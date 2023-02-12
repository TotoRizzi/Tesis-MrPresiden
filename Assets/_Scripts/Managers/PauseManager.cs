using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    GameManager _gameManager;

    bool _paused = false;

    private void Start()
    {
        _gameManager = GameManager.instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _paused = !_paused;

            if (_paused)
            {
                _gameManager.Player.PausePlayer();
                _gameManager.UiManager.ShowPauseMenu();
                Time.timeScale = 0;
            }
            else
            {
                _gameManager.Player.UnPausePlayer();
                _gameManager.UiManager.HidePauseMenu();
                Time.timeScale = 1;
            }
        }
    }
}
