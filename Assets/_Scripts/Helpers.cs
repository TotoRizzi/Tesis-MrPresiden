using UnityEngine;

public static class Helpers
{
    static GameManager _gameManager;
    static AudioManager _audioManager;
    static FullScreenMode _windowMode;
    static Resolution _currentResolution;
    public static GameManager GameManager
    {
        get
        {
            if (_gameManager == null) _gameManager = GameManager.instance;
            return _gameManager;
        }
    }
    public static AudioManager AudioManager
    {
        get
        {
            if (_audioManager == null) _audioManager = AudioManager.Instance;
            return _audioManager;
        }
    }

    public static Resolution CurrentResolution
    {
        get
        {
            if (_currentResolution.Equals(null)) _currentResolution = Screen.currentResolution;
            return _currentResolution;
        }
        set { _currentResolution = value; }
    }
    public static FullScreenMode WindowMode
    {
        get { return _windowMode; }
        set { _windowMode = value; }
    }
}
