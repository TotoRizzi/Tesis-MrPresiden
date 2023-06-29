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
    [SerializeField] Button _skipButton;

    KeysUI[] _keys;
    GameObject _info;
    bool _activated;
    float _pauseTime = 2;
    private void Start()
    {
        _info = GetComponentInChildren<Canvas>().gameObject;
        _skipButton.onClick.AddListener(() =>
        {
            Helpers.GameManager.PauseManager.TutorialPause();
            Destroy(gameObject);
        });
        _skipButton.interactable = false;
        if (_isTip)
        {
            _tipTxt.text = LanguageManager.Instance.selectedLanguage == Languages.eng ? _ENGText : _SPAText;
            return;
        }
        _keys = GetComponentsInChildren<KeysUI>();
        _inputActionTxt.text = LanguageManager.Instance.selectedLanguage == Languages.eng ? _ENGText : _SPAText;
        _info.SetActive(false);
    }
    IEnumerator Wait()
    {
        _activated = true;
        _info.SetActive(true);
        for (int i = 0; i < _inputsNames.Length; i++)
        {
            _keys[i].active = true;
            _keys[i].SetButtonSprite(_inputsNames[i]).SetDelay(i % 2 == 0);
        }
        Helpers.GameManager.PauseManager.TutorialPause();
        yield return new WaitForSeconds(_pauseTime);
        _skipButton.interactable = true;
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
