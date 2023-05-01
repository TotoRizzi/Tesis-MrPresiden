using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    EasyGameMode = 0,
    HardGameMode = 1
}

public class GameModeSelector : MonoBehaviour
{
    [SerializeField] GameModeManager easyGameMode, hardGameMode;

    int _myGameMode;
    private void Start()
    {
        _myGameMode = Helpers.GameManager.SaveDataManager.GetInt(Helpers.GameManager.gameModeSaveName, (int) GameMode.EasyGameMode);

         switch(_myGameMode)
         {
             case (int)GameMode.EasyGameMode:
                 Instantiate(easyGameMode, this.transform);
                 break;
             case (int)GameMode.HardGameMode:
                 Instantiate(hardGameMode, this.transform);
                 break;
             default:
                 Debug.LogWarning("GameMode NOT FOUND");
                 break;
         }
    }
}
