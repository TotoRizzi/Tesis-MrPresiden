using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LevelLightsManager : MonoBehaviour
{
    [SerializeField] Light2D[] _lights;
    [SerializeField] Color[] _colors;

    [SerializeField] float _lerpSpeed = .5f;

    private void Update()
    {
        foreach(var item in _lights)
            item.color = Color.Lerp(_colors[0], _colors[1], _lerpSpeed * Time.deltaTime);
    }
}
