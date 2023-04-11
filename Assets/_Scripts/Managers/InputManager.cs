using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    Dictionary<string, KeyCode> _buttonKeys;
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

        _buttonKeys["Move Right"] = KeyCode.D;
        _buttonKeys["Move Left"] = KeyCode.A;
        _buttonKeys["Move Up"] = KeyCode.W;
        _buttonKeys["Move Down"] = KeyCode.S;
        _buttonKeys["Jump"] = KeyCode.Space;
        _buttonKeys["Dash"] = KeyCode.LeftShift;
        _buttonKeys["Pick Up"] = KeyCode.E;
        _buttonKeys["Throw Weapon"] = KeyCode.G;
        _buttonKeys["Shoot"] = KeyCode.Mouse0;
        _buttonKeys["Melee"] = KeyCode.Mouse1;
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
    public void SetButtonForKey(string buttonName, KeyCode keyCode)
    {
        _buttonKeys[buttonName] = keyCode;
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