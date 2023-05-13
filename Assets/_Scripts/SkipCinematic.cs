using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipCinematic : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKey)
        {
            Helpers.GameManager.CinematicManager.SkipDefeatCinematic();
            gameObject.SetActive(false);
        }
    }
}
