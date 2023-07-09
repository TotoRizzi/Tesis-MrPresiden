using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TutorialControls : MonoBehaviour
{
    [SerializeField] bool _isTip;
    [SerializeField, Header("English"), TextArea(minLines: 1, maxLines: 4)] string _ENGText;
    [SerializeField, Header("Spanish"), TextArea(minLines: 1, maxLines: 4)] string _SPAText;
    [SerializeField] string[] _inputsNames;
    [SerializeField] TextMeshProUGUI _inputActionTxt, _tipTxt;
    [SerializeField] Image _fillImg;

    KeysUI[] _keys;
    GameObject _info;
    bool _activated;
    float _pauseTime = 2;
    System.Action _state = delegate { };
    InputManager _inputManager;
    private void Start()
    {
        _inputManager = InputManager.Instance;
        _info = GetComponentInChildren<Canvas>().gameObject;
        if (_isTip)
        {
            _tipTxt.text = LanguageManager.Instance.selectedLanguage == Languages.eng ? _ENGText : _SPAText;
            return;
        }
        _keys = GetComponentsInChildren<KeysUI>();
        _inputActionTxt.text = LanguageManager.Instance.selectedLanguage == Languages.eng ? _ENGText : _SPAText;
        _info.SetActive(false);
        _timer = _pauseTime;
    }
    private void Update()
    {
        _state?.Invoke();
    }
    IEnumerator Wait()
    {
        _activated = true;
        _info.SetActive(true);
        _state = TimePaused;
        for (int i = 0; i < _inputsNames.Length; i++)
        {
            _keys[i].active = true;
            _keys[i].SetDelay(i % 2 == 0).SetButtonSprite(_inputsNames[i]);
        }
        Helpers.GameManager.PauseManager.TutorialPause();
        yield return new WaitForSeconds(_pauseTime);
        _state = State;
    }

    float _timer;
    void TimePaused()
    {
        _timer -= Time.deltaTime;
        _fillImg.fillAmount = _timer / _pauseTime;
    }
    void State()
    {
        if (Input.anyKey)
        {
            foreach (var item in _inputsNames)
                if (_inputManager.GetButtonDown(item))
                {
                    Helpers.GameManager.PauseManager.TutorialPause();
                    EventManager.TriggerEvent(Contains.PLAYER_TUTORIAL_ACTION, item);
                    Destroy(gameObject);
                }
        }
    }
    private void OnTriggerEnter2D()
    {
        if (!_activated)
            StartCoroutine(Wait());
    }
    private void OnDestroy()
    {
        if (_isTip) return;
        for (int i = 0; i < _inputsNames.Length; i++)
            _keys[i].ReturnObject();
    }
}
