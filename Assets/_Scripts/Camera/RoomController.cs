using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    RoomBasedCamera _camera;
    List<Enemy> _enemiesInRoom = new List<Enemy>();

    private void Start()
    {
        _camera = RoomBasedCamera.instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _camera.MoveCamera(transform);     
    }
}
