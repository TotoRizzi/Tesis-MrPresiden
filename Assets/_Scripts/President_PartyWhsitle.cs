using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class President_PartyWhsitle : MonoBehaviour
{
    public void PlayPartyWhistle()
    {
        Helpers.AudioManager.PlaySFX("President_PartyWhistle");
    }
}
