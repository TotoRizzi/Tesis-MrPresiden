using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeSelector : MonoBehaviour
{
    [SerializeField] GameModeManager easyGameMode;


    private void Start()
    {
        Instantiate(easyGameMode, this.transform);
    }
}
