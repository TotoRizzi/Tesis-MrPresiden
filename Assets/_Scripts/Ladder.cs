using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    Collider2D _myCollider;
    Player _player;
    [SerializeField] float _colliderEnabledTime = .5f;



    private void Start()
    {
        _player = GameManager.instance.Player;
        _myCollider = GetComponent<BoxCollider2D>();

        _player.ExitClimb += BlockCollider;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _player.gameObject.transform.position = new Vector2(transform.position.x, collision.gameObject.transform.position.y);
        _player.Climb();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _player.StopClimging();
        BlockCollider();
    }

    void BlockCollider()
    {
        _myCollider.enabled = false;
        StartCoroutine(ReturnCollider());
    }

    IEnumerator ReturnCollider()
    {
        yield return new WaitForSeconds(_colliderEnabledTime);
        _myCollider.enabled = true;
    }
}
