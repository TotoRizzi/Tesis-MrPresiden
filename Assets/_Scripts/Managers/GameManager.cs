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
    [SerializeField] LayerMask _groundLayer, _playerLayer, _weaponLayer, _dynamicBodies;
    public LayerMask GroundLayer { get { return _groundLayer; } private set { } }
    public LayerMask PlayerLayer { get { return _playerLayer; } private set { } }
    public LayerMask WeaponLayer { get { return _weaponLayer; } private set { } }
    public LayerMask DynamicBodiesLayer { get { return _dynamicBodies; } private set { } }


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
