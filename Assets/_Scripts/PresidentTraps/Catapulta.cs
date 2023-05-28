using UnityEngine;
public class Catapulta : MonoBehaviour
{
    Vector3 _initialRotation;
    [SerializeField] Vector3 _targetRotation;
    void Start()
    {
        _initialRotation = transform.eulerAngles;   
    }
    void Update()
    {
        transform.eulerAngles = Vector3.Lerp(_initialRotation, _targetRotation, Helpers.LevelTimerManager.Timer / Helpers.LevelTimerManager.LevelMaxTime);
    }
}
