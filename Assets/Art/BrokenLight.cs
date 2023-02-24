using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenLight : MonoBehaviour
{
    public float minTime = 0.07f;
    public float maxTime = 0.3f;

    public GameObject targetObject;

    private bool isBlinking = false;

    void Start()
    {
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)
        {
            isBlinking = true;
            targetObject.SetActive(!targetObject.activeSelf);
            float blinkTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(blinkTime);
            isBlinking = false;
            yield return null;
        }
    }

    public bool IsBlinking()
    {
        return isBlinking;
    }
}
