using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRoom : MonoBehaviour
{
    RoomBasedCamera _camera;

    private void Start()
    {
        _camera = RoomBasedCamera.instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _camera.MoveCamera(transform);
    }
}
