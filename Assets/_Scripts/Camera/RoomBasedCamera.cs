using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBasedCamera : MonoBehaviour
{
    Vector3 _velocity;
    GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.instance;
    }

    public void MoveCamera(Transform pos)
    {
        transform.position = Vector3.SmoothDamp(transform.position, pos.position, ref _velocity, _gameManager.CameraSpeed);
    }
}
