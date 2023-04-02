using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteRotation : MonoBehaviour
{
    [SerializeField] float _speed = 30;

    float rotZ;

    private void Update()
    {
        rotZ += Time.deltaTime ;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}
