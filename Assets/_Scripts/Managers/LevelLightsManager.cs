using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LevelLightsManager : MonoBehaviour
{
    Action OnUpdate;
    
    LevelTimerManager _levelTimerManager;
    public LevelTimerManager LevelTimerManager { get { return _levelTimerManager; } private set { } }

    StateMachine _fsm;

    [SerializeField] Light2D[] _lights;
    public Light2D[] Lights { get { return _lights; } private set { } }

    [SerializeField] Color[] _colors;
    public Color[] Colors { get { return _colors; } private set { } }

    [SerializeField] float _lerpSpeed = .5f;
    public float LerpSpeed { get { return _lerpSpeed; } private set { } }


    [SerializeField] float _minLerpSpeed = .5f;

    [SerializeField] float _maxTimer = 2f;
    public float MaxTimer 
    { 
        get 
        { 
            return Mathf.Clamp((_maxTimer * ((Helpers.LevelTimerManager.LevelMaxTime - Helpers.LevelTimerManager.Timer) / Helpers.LevelTimerManager.LevelMaxTime)), _minLerpSpeed, _maxTimer); 
        } 
        private set { } 
    }

    [SerializeField] [Range(0.0f, 1.0f)] float _startBlinkingLights;

    [SerializeField] SpriteRenderer _onOffLight;
    public SpriteRenderer OnOffLight { get { return _onOffLight; }  private set { } }

    [SerializeField] Color[] _onOffLightColors;
    public Color[] OnOffLightColors { get { return _onOffLightColors; } private set { } }

    bool _lightsAreBlinking = false;
    public bool LightsAreBlinking { get { return _lightsAreBlinking; } private set { } }

    [SerializeField] BrokenLight[] _brokenLights;

    private void Start()
    {
        _levelTimerManager = Helpers.LevelTimerManager;

        _fsm = new StateMachine();

        _fsm.AddState(StateName.LIGHT_GoingRed, new GoingRed(_fsm, this));
        _fsm.AddState(StateName.LIGHT_GoingWhite, new GoingWhite(_fsm, this));
        _fsm.AddState(StateName.LIGHT_Normal, new GoingNormal(_fsm, this));
        _fsm.ChangeState(StateName.LIGHT_GoingRed);

        OnUpdate += _fsm.Update;
        OnUpdate += CheckForBlinkingLights;

        Helpers.GameManager.EnemyManager.OnEnemyKilled += () => _fsm.ChangeState(StateName.LIGHT_Normal);
        _onOffLight.color = _onOffLightColors[1];
    }

    private void Update()
    {
        OnUpdate?.Invoke();
    }

    void CheckForBlinkingLights()
    {
        if (Helpers.LevelTimerManager.Timer / Helpers.LevelTimerManager.LevelMaxTime >= _startBlinkingLights)
        {
            StartBlinkLights();
            OnUpdate -= CheckForBlinkingLights;
        }
    }

    public void StartBlinkLights()
    {
        _lightsAreBlinking = true;
        foreach (var item in _brokenLights)
        {
            item.CanBlink();
        }
    }

    public void StopBLinkingLights()
    {
        foreach (var item in _brokenLights)
        {
            item.CantBlink();
        }
    }
}

public class GoingRed : IState
{
    StateMachine _fsm;
    LevelLightsManager _manager;

    float _currentTimer;

    public GoingRed(StateMachine fsm, LevelLightsManager manager)
    {
        _fsm = fsm;
        _manager = manager;
    }

    public void OnEnter()
    {
        _currentTimer = 0;
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
        _currentTimer += Time.deltaTime;

        foreach (var item in _manager.Lights)
        {
            Color initialColor = item.color;

            if(_currentTimer < 1)
            {
                item.color = Color.Lerp(initialColor, _manager.Colors[1], _currentTimer * _manager.LerpSpeed);
            }
        }

        if (_currentTimer >= _manager.MaxTimer) _fsm.ChangeState(StateName.LIGHT_GoingWhite);
    }
}
public class GoingWhite : IState
{
    StateMachine _fsm;
    LevelLightsManager _manager;

    float _currentTimer;

    public GoingWhite(StateMachine fsm, LevelLightsManager manager)
    {
        _fsm = fsm;
        _manager = manager;
    }

    public void OnEnter()
    {
        _currentTimer = 0;
    }

    public void OnExit()
    {
    }

    public void OnFixedUpdate()
    {
    }

    public void OnUpdate()
    {
        _currentTimer += Time.deltaTime;

        foreach (var item in _manager.Lights)
        {
            Color initialColor = item.color;

            if (_currentTimer < 1)
            {
                item.color = Color.Lerp(initialColor, _manager.Colors[0], _currentTimer * _manager.LerpSpeed);
            }
        }

        if (_currentTimer >= _manager.MaxTimer) _fsm.ChangeState(StateName.LIGHT_GoingRed);
    }
}
public class GoingNormal : IState
{
    StateMachine _fsm;
    LevelLightsManager _manager;

    float _currentTimer;

    public GoingNormal(StateMachine fsm, LevelLightsManager manager)
    {
        _fsm = fsm;
        _manager = manager;
    }

    public void OnEnter()
    {
        _currentTimer = 0;

        if (_manager.LightsAreBlinking) _manager.StopBLinkingLights();
        _manager.OnOffLight.color = _manager.OnOffLightColors[0];

        foreach (var item in _manager.Lights)
        {
            item.color = _manager.Colors[0];
        }
    }

    public void OnExit()
    {
        if(_manager.LightsAreBlinking) _manager.StartBlinkLights();

        _manager.OnOffLight.color = _manager.OnOffLightColors[1];
    }

    public void OnFixedUpdate()
    {

    }

    public void OnUpdate()
    {
        _currentTimer += Time.deltaTime;

        if (_currentTimer >= Helpers.LevelTimerManager.TimeToDiscount) _fsm.ChangeState(StateName.LIGHT_GoingRed);
    }
}