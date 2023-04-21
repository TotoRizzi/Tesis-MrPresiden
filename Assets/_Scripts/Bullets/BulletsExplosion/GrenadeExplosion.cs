using System.Collections;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{
    [SerializeField] float _timeToReturn;
    void Start()
    {
        StartCoroutine(DisableObject());
    }

    IEnumerator DisableObject()
    {
        float timer = 0;
        while(timer <= _timeToReturn)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        ReturnObject();
    }

    void ReturnObject()
    {

    }
}
