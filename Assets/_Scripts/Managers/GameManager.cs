using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Layers")]
    [SerializeField] LayerMask _groundLayer;
    public LayerMask GroundLayer { get { return _groundLayer; } private set { } }

    Player _player;
    public Player Player { get { return _player; } private set { } }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        _player = FindObjectOfType<Player>();

    }

}
