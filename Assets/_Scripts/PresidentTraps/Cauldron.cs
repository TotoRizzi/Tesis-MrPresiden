using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    [SerializeField] Color _colorToChange;
    [SerializeField] SpriteRenderer _ObjectToChange;

    private void Update()
    {
        var color = Color.Lerp(Color.white, _colorToChange, Helpers.LevelTimerManager.Timer / Helpers.LevelTimerManager.LevelMaxTime);
        _ObjectToChange.color = color;
    }


}
