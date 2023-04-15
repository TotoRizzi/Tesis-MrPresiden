using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LevelLightsManager : MonoBehaviour
{
    [SerializeField] Light2D[] _lights;
    public Light2D[] Lights { get { return _lights; } private set { } }

    [SerializeField] Color[] _colors;
    public Color[] Colors { get { return _colors; } private set { } }

    [SerializeField] float _lerpSpeed = .5f;
    public float LerpSpeed { get { return _lerpSpeed; } private set { } }

    LevelTimerManager _levelTimerManager;
    public LevelTimerManager LevelTimerManager { get { return _levelTimerManager; } private set { } }

    [SerializeField] float _maxTimer = 2f;
    public float MaxTimer 
    { 
        get 
        { 
            return _maxTimer * ((Helpers.LevelTimerManager.LevelMaxTime - Helpers.LevelTimerManager.Timer) / Helpers.LevelTimerManager.LevelMaxTime); 
        } 
        private set { } 
    }

    StateMachine _fsm;

    private void Start()
    {
        _levelTimerManager = Helpers.LevelTimerManager;

        _fsm = new StateMachine();

        _fsm.AddState(StateName.LIGHT_GoingRed, new GoingRed(_fsm, this));
        _fsm.AddState(StateName.LIGHT_GoingWhite, new GoingWhite(_fsm, this));
        _fsm.ChangeState(StateName.LIGHT_GoingRed);
    }

    private void Update()
    {
        _fsm.Update();
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