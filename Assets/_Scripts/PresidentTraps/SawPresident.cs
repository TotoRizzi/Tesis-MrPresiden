using UnityEngine;
public class SawPresident : MonoBehaviour
{
    [SerializeField] Transform _targetPosition;
    Vector3 _initialPosition;
    [SerializeField] Animator _animator;
    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _initialPosition = transform.position;
        Helpers.LevelTimerManager.RedButton += StopSaw;
    }
    private void OnDisable()
    {
        //Helpers.LevelTimerManager.RedButton -= StopSaw;
    }
    void Update()
    {
        transform.position = Vector3.Lerp(_initialPosition, _targetPosition.position, Helpers.LevelTimerManager.Timer / Helpers.LevelTimerManager.LevelMaxTime);
    }
    public void StopSaw()
    {
        Debug.Log("Empty");
        _animator.Play("Empty");
    }
}
