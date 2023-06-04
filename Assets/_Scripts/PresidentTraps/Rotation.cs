using System.Collections;
using UnityEngine;
public class Rotation : MonoBehaviour
{
    [SerializeField, Tooltip("Target rotation in degrees")] float _rotationAmount;
    [SerializeField, Tooltip("Rotate Direction")] Vector3 _rotateDirection;
    void Start()
    {
        StartCoroutine(Rotate());
    }
    IEnumerator Rotate()
    {
        Quaternion startRot = transform.rotation;
        while (Helpers.LevelTimerManager.Timer / Helpers.LevelTimerManager.LevelMaxTime <= 1)
        {
            transform.rotation = startRot * Quaternion.AngleAxis(Helpers.LevelTimerManager.Timer / Helpers.LevelTimerManager.LevelMaxTime * _rotationAmount, _rotateDirection);
            yield return null;
        }
    }
}
