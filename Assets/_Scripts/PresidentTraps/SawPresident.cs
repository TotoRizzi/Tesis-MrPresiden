using UnityEngine;
public class SawPresident : MonoBehaviour
{
    [SerializeField] Transform _targetPosition;
    Vector3 _initialPosition;
    private void Start()
    {
        _initialPosition = transform.position;
    }
    void Update()
    {
        transform.position = Vector3.Lerp(_initialPosition, _targetPosition.position, Helpers.LevelTimerManager.Timer / Helpers.LevelTimerManager.LevelMaxTime);
    }
}
