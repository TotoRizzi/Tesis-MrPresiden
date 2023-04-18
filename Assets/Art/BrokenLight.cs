using UnityEngine;
using System;

public class BrokenLight : MonoBehaviour
{
    Action OnUpdate;

    [SerializeField] float _minTime = 0.07f;
    [SerializeField] float _maxTime = 0.3f;

    [SerializeField] GameObject _targetObject;

    [SerializeField] float _blinkCdTime = 2;
    [SerializeField] float _realCdTimeMultiplier = 2;
    float _realBlinkCdTime;
    float _currentBlinkCdTime;

    float _blinkTime;
    float _currentBlinkingTimer;
    float _blinkCount;

    bool _canBlink;

    private void Start()
    {
        OnUpdate += CheckBlink;
    }
    private void Update()
    {
        if (_canBlink) OnUpdate?.Invoke();
    }

    void CheckBlink()
    {
        _currentBlinkingTimer += Time.deltaTime;

        if (_currentBlinkingTimer < _blinkTime) return;

        _targetObject.SetActive(!_targetObject.activeSelf);

        _blinkCount++;
        _currentBlinkingTimer = 0;
        _blinkTime = UnityEngine.Random.Range(_minTime, _maxTime);

        if(_blinkCount == 2)
        {
            _blinkCount = 0;
            _realBlinkCdTime = UnityEngine.Random.Range(_blinkCdTime / _realCdTimeMultiplier, _blinkCdTime * _realCdTimeMultiplier);
            OnUpdate -= CheckBlink;
            OnUpdate += ReturnBlink;
        }
    }

    void ReturnBlink()
    {
        _currentBlinkCdTime += Time.deltaTime;

        if (_currentBlinkCdTime < _realBlinkCdTime) return;

        _currentBlinkCdTime = 0;

        OnUpdate -= ReturnBlink;
        OnUpdate += CheckBlink;
    }

    public void CanBlink()
    {
        _canBlink = true;
        _currentBlinkingTimer = 0;
    }

    public void CantBlink()
    {
        _canBlink = false;
        TurnOnLights();
        _currentBlinkingTimer = 0;
    }

    void TurnOnLights()
    {
        _targetObject.SetActive(true);
    }
}
