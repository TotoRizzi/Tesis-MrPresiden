public static class Helpers
{
    static GameManager _gameManager;
    static AudioManager _audioManager;
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
}
