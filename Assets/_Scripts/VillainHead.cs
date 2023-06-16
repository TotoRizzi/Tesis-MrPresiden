using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillainHead : MonoBehaviour
{
    GeneralPlayer _player;
    [SerializeField] Transform _headContainer;
    Vector3 _deafaultScale;

    private void Start()
    {
        _player = Helpers.GameManager.Player;
        _deafaultScale = _headContainer.localScale;
    }

    private void Update()
    {
        //MoveHead();
    }

    void MoveHead()
    {
        Vector3 dirToLookAt = (_player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dirToLookAt.y, dirToLookAt.x) * Mathf.Rad2Deg;

        _headContainer.eulerAngles = new Vector3(0, 0, angle);

        Vector3 newScale = _deafaultScale;

        if (angle > 90 || angle < -90)
        {
            newScale.y = -_deafaultScale.y;
        }
        else
        {
            newScale.y = _deafaultScale.y;
        }

        _headContainer.localScale = newScale;
    }
}
