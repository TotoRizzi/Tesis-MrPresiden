using UnityEngine;
using System;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public event Action OnGameLost;
    public event Action OnGameWon;
    public event Action OnRoomWon;
    public event Action OnPlayerDead;

    public Action SetPlayerSkin = delegate { }, SetPresidentSkin = delegate { };

    [Header("Layers")]
    [SerializeField] LayerMask _groundLayer, _playerLayer, _weaponLayer, _dynamicBodies, _borderLayer, _invisibleWallLayer;
    public LayerMask GroundLayer { get { return _groundLayer; } private set { } }
    public LayerMask BorderLayer { get { return _borderLayer; } private set { } }
    public LayerMask PlayerLayer { get { return _playerLayer; } private set { } }
    public LayerMask WeaponLayer { get { return _weaponLayer; } private set { } }
    public LayerMask InvisibleWallLayer { get { return _invisibleWallLayer; } private set { } }
    public LayerMask DynamicBodiesLayer { get { return _dynamicBodies; } private set { } }


    [Header("Camera")]
    [SerializeField] float _cameraShakeDuration;
    public float CameraShakeDuration { get { return _cameraShakeDuration; } private set { } }

    [SerializeField] float _cameraSpeed;
    public float CameraSpeed { get { return _cameraSpeed; } private set { } }

    GeneralPlayer _player;
    public GeneralPlayer Player { get { return _player; } private set { } }

    [Header("GameModes")]
    [SerializeField] int _defaultHardLives = 3;
    public int DefaultHardLives { get { return _defaultHardLives; } private set { } }

    [HideInInspector] public string gameModeSaveName = "GameMode";


    [SerializeField] LoadSceneManager _loadSceneManager;

    public LoadSceneManager LoadSceneManager { get { return _loadSceneManager; } private set { } }

    private BaseEnemyManager _enemyManager;
    public BaseEnemyManager EnemyManager { get { return _enemyManager; } private set { } }

    private EffectsManager _effectsManager;
    public EffectsManager EffectsManager { get { return _effectsManager; } private set { } }

    private UiManager _uiManager;
    public UiManager UiManager { get { return _uiManager; } private set { } }

    private SaveDataManager _saveDataManager;
    public SaveDataManager SaveDataManager { get { return _saveDataManager; } private set { } }

    private Scene_Manager _sceneManager;
    public Scene_Manager SceneManager { get { return _sceneManager; } private set { } }

    private PauseManager _pauseManager;
    public PauseManager PauseManager { get { return _pauseManager; } private set { } }

    DropManager _dropManager;
    public DropManager DropManager { get { return _dropManager; } }

    StatisticsManager _statisticsManager;
    public StatisticsManager StatisticsManager { get { return _statisticsManager; } }

    CinematicManager _cinematicManager;
    public CinematicManager CinematicManager { get { return _cinematicManager; } }

    GameModeManager _gameMode;
    public GameModeManager GameMode { get { return _gameMode; } private set { } }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        _player = FindObjectOfType<GeneralPlayer>();
        _enemyManager = GetComponent<BaseEnemyManager>();
        if (_enemyManager == null) _enemyManager = FindObjectOfType<BaseEnemyManager>();
        _effectsManager = GetComponent<EffectsManager>();
        _uiManager = GetComponent<UiManager>();
        _saveDataManager = GetComponent<SaveDataManager>();
        _pauseManager = GetComponent<PauseManager>();
        _sceneManager = GetComponent<Scene_Manager>();
        _dropManager = GetComponent<DropManager>();
        _statisticsManager = GetComponent<StatisticsManager>();
        _cinematicManager = GetComponent<CinematicManager>();
        Cursor.lockState = CursorLockMode.Confined;

        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        //_gameMode = GameObject.Find("GameMode").GetComponentInChildren<GameModeManager>();
    }

    public void RoomWon()
    {
        OnRoomWon?.Invoke();
    }
    public void GameWon()
    {
        OnGameWon?.Invoke();
    }
    public void GameLost()
    {
        OnGameLost?.Invoke();
    }
    public void PlayerDead()
    {
        OnPlayerDead?.Invoke();
    }
}
