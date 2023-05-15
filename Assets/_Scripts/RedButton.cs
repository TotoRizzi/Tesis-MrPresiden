using System.Collections;
using UnityEngine;
public class RedButton : MonoBehaviour
{
    Collider2D _collider;
    Animator _anim;
    bool _isPlayerOnTrigger;
    InputManager _inputManager;

    //[SerializeField] GameObject lightParent;
    [SerializeField] SpriteRenderer _buttonSprite;
    private void Start()
    {
        Helpers.GameManager.OnRoomWon += ShowExit;
        Helpers.GameManager.OnPlayerDead += () => StartCoroutine(HideExit());

        _inputManager = InputManager.Instance;
        _collider = GetComponent<Collider2D>();
        _anim = GetComponentInChildren<Animator>();
        StartCoroutine(HideExit());
    }
    private void Update()
    {
        if (_inputManager.GetButtonDown("Pick Up") && _isPlayerOnTrigger) PlayRedButton(); 
    }
    void ShowExit()
    {
        _anim.SetBool("IsOpen", true);
        _collider.enabled = true;
        //lightParent.SetActive(true);
        _buttonSprite.color = Color.red;
    }
    IEnumerator HideExit()
    {
        yield return new WaitForEndOfFrame();
        _collider.enabled = false;
        _anim.SetBool("IsOpen", false);
        //lightParent.SetActive(false);
        _buttonSprite.color = Color.white;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<WeaponManager>()) _isPlayerOnTrigger = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<WeaponManager>()) _isPlayerOnTrigger = false;
    }
    void PlayRedButton()
    {
        Helpers.LevelTimerManager.RedButton();
        Helpers.GameManager.CinematicManager.PlayVictoryCinematic();
    }
}
