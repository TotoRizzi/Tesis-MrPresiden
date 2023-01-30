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
        if(collision.tag == "Player")
        {
            _camera.MoveCamera(transform);
            foreach (var item in _enemiesInRoom)
            {
                if (item)
                    item.enabled = true;
            }
        }
        else
        {
            var enemy = collision.GetComponent<Enemy>();
            _enemiesInRoom.Add(enemy);
            enemy.enabled = false;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;

        foreach (var item in _enemiesInRoom)
        {
            if(item)
                item.enabled = false;
        }
    }
}
