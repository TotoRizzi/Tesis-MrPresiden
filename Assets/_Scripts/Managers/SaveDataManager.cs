using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.OnGameLost += Reset;
    }

    public void SaveFloat(string name, float value)
    {
        PlayerPrefs.SetFloat(name, value);
    }

    public float GetFloat(string name, float defaultValue)
    {
        return PlayerPrefs.HasKey(name) ? PlayerPrefs.GetFloat(name, defaultValue) : defaultValue;
    }

    void Reset()
    {
        PlayerPrefs.DeleteAll();
    }
}
