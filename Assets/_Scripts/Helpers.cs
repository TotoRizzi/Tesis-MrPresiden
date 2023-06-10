using UnityEngine;

public static class Helpers
{
    static GameManager _gameManager;
    static AudioManager _audioManager;
    static LevelTimerManager _levelTimerManager;
    static PersistantData _persistantData;
    static Camera _mainCamera;
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
    public static LevelTimerManager LevelTimerManager
    {
        get
        {
            if (_levelTimerManager == null) _levelTimerManager = GameObject.FindObjectOfType<LevelTimerManager>();
            return _levelTimerManager;
        }
    }
    public static PersistantData PersistantData
    {
        get
        {
            if (_persistantData == null) _persistantData = PersistantData.Instance;
            return _persistantData;
        }
    }
    public static Camera MainCamera
    {
        get
        {
            if (_mainCamera == null) _mainCamera = Camera.main;
            return _mainCamera;
        }
    }
}
