using UnityEngine;
public class Malo : MonoBehaviour
{
    Animator _animator;
    [SerializeField] string _winAnimationName, _onEnemyKilledAnimation;
    void Start()
    {
        _animator = GetComponent<Animator>();
        Helpers.GameManager.EnemyManager.OnEnemyKilled += () => _animator.Play(_onEnemyKilledAnimation);
        Helpers.LevelTimerManager.OnLevelDefeat += () => _animator.Play(_winAnimationName);
    }
}
