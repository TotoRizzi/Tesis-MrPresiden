using UnityEngine;
public class Aplanadora : MonoBehaviour
{
    [SerializeField] Transform[] _weels;
    [SerializeField] Transform _aplanadoraWeel;
    [SerializeField] float _speed;

    bool _stop;
    LevelTimerManager _levelTimerManager;
    Vector3 _dir = -Vector3.forward;
    private void Start()
    {
        _levelTimerManager = Helpers.LevelTimerManager;
        Helpers.LevelTimerManager.OnLevelStart += () => _stop = true;
    }
    private void Update()
    {
        bool stopWeel = _levelTimerManager.TrapStopped || _stop;

        for (int i = 0; i < _weels.Length; i++)
            _weels[i].Rotate(stopWeel ? _dir * _speed : Vector3.zero);

        _aplanadoraWeel.Rotate(stopWeel ? _dir * _speed * .25f : Vector3.zero);
    }
    public void ReverseSpeed() { _speed *= -1; }
    public void StopAplanadora() { _speed = 0; }
}
