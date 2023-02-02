using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    public Sound[] soundEffects;
    public AudioSource soundEffectSource;

    public void PlaySound(string name)
    {
        Sound s = Array.Find(soundEffects, x => x.soundName == name);

        if (s == null) Debug.LogWarning(name + " Sound NOT found");
        else soundEffectSource.PlayOneShot(s.clip);
    }
}