using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.OnGameLost += Reset;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.O)) Reset();
    }

    public void SaveFloat(string name, float value)
    {
        PlayerPrefs.SetFloat(name, value);
    }
    public float GetFloat(string name, float defaultValue)
    {
        return PlayerPrefs.HasKey(name) ? PlayerPrefs.GetFloat(name, defaultValue) : defaultValue;
    }

    public void SaveBool(string name, bool value)
    {
        int newValue = value ? 1 : 0;

        PlayerPrefs.SetInt(name, newValue);
    }
    public bool GetBool(string name, bool defaultValue)
    {
        if (!PlayerPrefs.HasKey(name)) return defaultValue;

        bool newValue = PlayerPrefs.GetInt(name) == 1 ? true : false;

        return newValue;     
    }

    public void SaveInt(string name, int value)
    {
        PlayerPrefs.SetInt(name, value);
    }
    public int GetInt(string name, int defaultValue)
    {
        return PlayerPrefs.HasKey(name) ? PlayerPrefs.GetInt(name, defaultValue) : defaultValue;
    }

    void Reset()
    {
        Debug.Log("Reset");
        PlayerPrefs.DeleteAll();
    }
}
