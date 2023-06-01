using UnityEngine;
public class Catapulta : MonoBehaviour
{
    Quaternion _initialQuaterion;
    [SerializeField ]Quaternion _targetQuaternion;
    void Start()
    {
        _initialQuaterion = transform.rotation;   
    }
    void Update()
    {
        transform.rotation = Quaternion.Slerp(_initialQuaterion, _targetQuaternion, Helpers.LevelTimerManager.Timer / Helpers.LevelTimerManager.LevelMaxTime);
    }
}
