using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Layers
{
    PlayerAttack = 11,
    EnemyAttack = 12
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public event Action OnGameLost;
    public event Action OnGameWon;
    public event Action OnAchievementReached;

    [Header("Layers")]
    [SerializeField] LayerMask _groundLayer, _playerLayer, _weaponLayer, _dynamicBodies;
    public LayerMask GroundLayer { get { return _groundLayer; } private set { } }
    public LayerMask PlayerLayer { get { return _playerLayer; } private set { } }
    public LayerMask WeaponLayer { get { return _weaponLayer; } private set { } }
    public LayerMask DynamicBodiesLayer { get { return _dynamicBodies; } private set { } }


    [Header("Camera")]
    [SerializeField] float _cameraShakeDuration;
    public float CameraShakeDuration { get { return _cameraShakeDuration; } private set { } }

    [SerializeField] float _cameraSpeed;
    public float CameraSpeed { get { return _cameraSpeed; } private set { } }

    [Header("Combos")]
    [SerializeField] float _comboExpireTime = 4f;
    public float ComboExpireTime { get { return _comboExpireTime; } private set { } }

    [SerializeField] float _pointsForFirstAchievement = 50;
    public float PointsForFirstAchievement { get { return _pointsForFirstAchievement; } private set { } }

    [SerializeField] float _achievementPointsMultiplier = 1.5f;
    public float AchievementPointsMultiplier { get { return _achievementPointsMultiplier; } private set { } }

    private List<Action> _playerAchievements = new List<Action>();

    [Header("Enemies")]
    [SerializeField] int _pointsPerEnemy = 10;
    public int PointsPerEnemy { get { return _pointsPerEnemy; } private set { } }

    Player _player;
    public Player Player { get { return _player; } private set { } }

    private ComboManager _comboManager;
    public ComboManager ComboManager { get { return _comboManager; } private set { } }

    private EnemyManager _enemyManager;
    public EnemyManager EnemyManager { get { return _enemyManager; } private set { } }

    private EffectsManager _effectsManager;
    public EffectsManager EffectsManager { get { return _effectsManager; } private set { } }

    private SoundManager _soundManager;
    public SoundManager SoundManager { get { return _soundManager; } private set { } }
    
    private UiManager _uiManager;
    public UiManager UiManager { get { return _uiManager; } private set { } }

    private SaveDataManager _saveDataManager;
    public SaveDataManager SaveDataManager { get { return _saveDataManager; } private set { } }

    private Scene_Manager _sceneManager;
    public Scene_Manager SceneManager { get { return _sceneManager; } private set { } }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        _player = FindObjectOfType<Player>();
        _comboManager = GetComponent<ComboManager>();
        _enemyManager = GetComponent<EnemyManager>();
        _effectsManager = GetComponent<EffectsManager>();
        _soundManager = GetComponent<SoundManager>();
        _uiManager = GetComponent<UiManager>();
        _saveDataManager = GetComponent<SaveDataManager>();
        _sceneManager = GetComponent <Scene_Manager>();
    }

    public void LevelUpPlayer()
    {

    }
    public void GameWon()
    {
        OnGameWon();
    }
    public void GameLost()
    {
        OnGameLost();
    }

    public void AddAchievement(Action achievent)
    {
        _playerAchievements.Add(achievent);
    }

    public void GiveAchievement(int index)
    {
        if (index > _playerAchievements.Count - 1) return;

        _playerAchievements[index]();
        OnAchievementReached?.Invoke();
    }
}
