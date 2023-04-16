using UnityEngine;

public class BrokenLight : MonoBehaviour
{

    [SerializeField] float _minTime = 0.07f;
    [SerializeField] float _maxTime = 0.3f;

    [SerializeField] GameObject _targetObject;

    float _currentBlinkingTimer;

    float _blinkTime;

    bool _canBlink;

    private void Update()
    {
        if (_canBlink) CheckBlink();
    }

    void CheckBlink()
    {
        _currentBlinkingTimer += Time.deltaTime;

        if (_currentBlinkingTimer < _blinkTime) return;

        _targetObject.SetActive(!_targetObject.activeSelf);

        _currentBlinkingTimer = 0;
        _blinkTime = UnityEngine.Random.Range(_minTime, _maxTime);
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
