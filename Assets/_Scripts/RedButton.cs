using System.Collections;
using UnityEngine;
public class RedButton : MonoBehaviour
{
    [SerializeField] GameObject lightParent;

    Collider2D _collider;
    Animator _anim;
    private void Start()
    {
        Helpers.GameManager.OnRoomWon += ShowExit;
        Helpers.GameManager.OnPlayerDead += () => StartCoroutine(HideExit());

        _collider = GetComponent<Collider2D>();
        _anim = GetComponentInChildren<Animator>();
        StartCoroutine(HideExit());
    }
    void ShowExit()
    {
        _anim.SetBool("IsOpen", true);
        _collider.enabled = true;
        lightParent.SetActive(true);
    }
    IEnumerator HideExit()
    {
        yield return new WaitForEndOfFrame();
        lightParent.SetActive(false);
        _collider.enabled = false;
        _anim.SetBool("IsOpen", false);
        Debug.Log("Closed");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Helpers.LevelTimerManager.RedButton();
        Helpers.GameManager.CinematicManager.PlayVictoryCinematic();
    }
}
