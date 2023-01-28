using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _maxDistance;

    float _currentDistance;
    float _dmg;


    private void Update()
    {
        transform.position += transform.right * _speed * Time.deltaTime;
        _currentDistance += Time.deltaTime;
        if (_currentDistance > _maxDistance)
        {
            ReturnBullet();
        }

    }

    public Bullet SetRotation(Quaternion rot)
    {
        transform.rotation = rot;
        return this;
    }
    public Bullet SetPosition(Vector3 pos)
    {
        transform.position = pos;
        return this;
    }

    private void Reset()
    {
        _currentDistance = 0;
    }
    public static void TurnOn(Bullet b)
    {
        b.Reset();
        b.gameObject.SetActive(true);
    }

    public static void TurnOff(Bullet b)
    {
        b.gameObject.SetActive(false);
    }
    void ReturnBullet()
    {
        FRY_Bullet.Instance.ReturnBullet(this);
    }
}
