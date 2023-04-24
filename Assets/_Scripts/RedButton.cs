using System.Collections;
using UnityEngine;
public class RedButton : MonoBehaviour
{
    Collider2D _collider;
    Animator _anim;

    [SerializeField] GameObject lightParent;
    [SerializeField] SpriteRenderer _buttonSprite;
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
        _buttonSprite.color = Color.red;
    }
    IEnumerator HideExit()
    {
        yield return new WaitForEndOfFrame();
        _collider.enabled = false;
        _anim.SetBool("IsOpen", false);
        lightParent.SetActive(false);
        _buttonSprite.color = Color.white;
    }
    private void OnTriggerEnter2D( )
    {
        Helpers.LevelTimerManager.RedButton();
        Helpers.GameManager.PauseManager.PauseObjectsInCinematic();
        Helpers.GameManager.CinematicManager.PlayVictoryCinematic();
    }
}
