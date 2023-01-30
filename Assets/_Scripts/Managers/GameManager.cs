using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Layers
{
    PlayerAttack = 11,
    EnemyAttack = 12
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Layers")]
    [SerializeField] LayerMask _groundLayer;
    public LayerMask GroundLayer { get { return _groundLayer; } private set { } }

    [SerializeField] LayerMask _playerLayer;
    public LayerMask PlayerLayer { get { return _playerLayer; } private set { } }

    [SerializeField] LayerMask _weaponLayer;
    public LayerMask WeaponLayer { get { return _weaponLayer; } }


    [Header("Camera")]
    [SerializeField] float _cameraShakeDuration;
    public float CameraShakeDuration { get { return _cameraShakeDuration; } private set { } }

    [SerializeField] float _cameraSpeed;
    public float CameraSpeed { get { return _cameraSpeed; } private set { } }

    Player _player;
    public Player Player { get { return _player; } private set { } }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        _player = FindObjectOfType<Player>();
    }

}
