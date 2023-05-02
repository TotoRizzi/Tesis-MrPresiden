using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
public class ControlSettings : MonoBehaviour
{
    InputManager _inputManager;
    [SerializeField] GameObject _keyItemPrefab, _keyWarning;
    [SerializeField] Transform _parentKeysItems;

    bool _isKeyWarningOn;
    string _keyToRebind = null;
    Color _originalColor;
    Dictionary<string, Button> _buttons = new Dictionary<string, Button>();
    void Start()
    {
        _inputManager = FindObjectOfType<InputManager>();
        string[] buttonNames = _inputManager.GetButtonNames();
        for (int i = 0; i < buttonNames.Length; i++)
        {
            string bn = buttonNames[i];
            var go = Instantiate(_keyItemPrefab);
            go.transform.SetParent(_parentKeysItems);
            go.transform.localScale = Vector3.one;

            var buttonName = go.transform.Find("ButtonName");
            TextToTranslate language = buttonName.GetComponent<TextToTranslate>();
            language.ID = "ID_" + bn.Replace(" ", string.Empty);

            Button button = go.transform.Find("Button").GetComponent<Button>();
            Image buttonImg = button.GetComponent<Image>();
            buttonImg.sprite = _inputManager.KeySpriteForButton(ref bn);

            _buttons[bn] = button;

            button.onClick.AddListener(() =>
            {
                buttonImg.color = Color.gray;
                StartRebindFor(bn);
            });

            _originalColor = buttonImg.color;
        }
    }
    private void Update()
    {
        if (_keyToRebind == null) return;

        if (Input.anyKey)
        {
            foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kc))
                {
                    if (_inputManager.KeysAllowed.Contains(kc))
                    {
                        SetKey(kc);
                        break;
                    }
                    else
                    {
                        if(!_isKeyWarningOn) StartCoroutine(KeyWarning());
                        break;
                    }
                }
            }
        }
    }
    void StartRebindFor(string buttonName)
    {
        _keyToRebind = buttonName;
    }

    void SetKey(KeyCode kc)
    {
        Sprite sprite = _inputManager.keysData.FirstOrDefault(x => x.input == kc).keySprite;
        _inputManager.SetButtonForKey(_keyToRebind, kc, sprite);
        _buttons[_keyToRebind].GetComponent<Image>().color = _originalColor;
        _buttons[_keyToRebind].GetComponent<Image>().sprite = sprite;
        _keyToRebind = null;
    }
    IEnumerator KeyWarning()
    {
        _isKeyWarningOn = true;
        _keyWarning.SetActive(true);
        yield return new WaitForSeconds(2);
        _keyWarning.SetActive(false);
        _isKeyWarningOn = false;
    }
}