using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public KeyData[] keysData;
    Dictionary<string, KeyCode> _buttonKeys;
    Dictionary<string, Sprite> _buttonKeysData;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    private void OnEnable()
    {
        _buttonKeys = new Dictionary<string, KeyCode>();
        _buttonKeysData = new Dictionary<string, Sprite>();

        keysData = Resources.LoadAll<KeyData>("KeysSO");

        _buttonKeys["Move Right"] = KeyCode.D;
        _buttonKeys["Move Left"] = KeyCode.A;
        _buttonKeys["Move Up"] = KeyCode.W;
        _buttonKeys["Move Down"] = KeyCode.S;
        _buttonKeys["Jump"] = KeyCode.Space;
        _buttonKeys["Dash"] = KeyCode.LeftShift;
        _buttonKeys["Pick Up"] = KeyCode.E;
        //_buttonKeys["Throw Weapon"] = KeyCode.G;
        _buttonKeys["Shoot"] = KeyCode.Mouse0;
        _buttonKeys["Knife"] = KeyCode.Mouse1;

        foreach (var item in _buttonKeys)
        {
            foreach (var key in keysData)
            {
                if(key.input == item.Value)
                {
                    _buttonKeysData.Add(item.Key, key.keySprite);
                    continue;
                }
            }
        }
    }
    public bool GetButtonDown(string buttonName)
    {
        if (!_buttonKeys.ContainsKey(buttonName)) return false;

        return Input.GetKeyDown(_buttonKeys[buttonName]);
    }
    public bool GetButtonUp(string buttonName)
    {
        if (!_buttonKeys.ContainsKey(buttonName)) return false;

        return Input.GetKeyUp(_buttonKeys[buttonName]);
    }
    public bool GetButton(string buttonName)
    {
        if (!_buttonKeys.ContainsKey(buttonName)) return false;

        return Input.GetKey(_buttonKeys[buttonName]);
    }
    public string[] GetButtonNames()
    {
        return _buttonKeys.Keys.ToArray();
    }
    public string KeyNameForButton(string buttonName)
    {
        if (!_buttonKeys.ContainsKey(buttonName)) return "N/A";

        return _buttonKeys[buttonName].ToString();
    }
    public Sprite KeySpriteForButton(ref string buttonName)
    {
        if (!_buttonKeysData.ContainsKey(buttonName))
        {
            Debug.Log(buttonName);
            Debug.Log("No lo contiene");
            return null;
        }

        string spriteName = _buttonKeysData[buttonName].name;

        return Resources.Load<Sprite>($"Keys/{spriteName}"); ;
    }
    public void SetButtonForKey(string buttonName, KeyCode keyCode, Sprite buttonSprite)
    {
        _buttonKeys[buttonName] = keyCode;
        _buttonKeysData[buttonName] = buttonSprite;
    }
    public float GetAxisRaw(string axis)
    {
        if (axis == "Horizontal")
            return GetButton("Move Right") ? 1f : GetButton("Move Left") ? -1f : 0;
        else if (axis == "Vertical")
            return GetButton("Move Up") ? 1f : GetButton("Move Down") ? -1f : 0;
        else return default;
    }
}