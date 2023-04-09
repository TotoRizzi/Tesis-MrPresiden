using UnityEngine;
using System.Collections;
public class NextSceneOnTrigger : MonoBehaviour
{
    [SerializeField] string _nextScene;
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

    }
    private void OnTriggerEnter2D()
    {
        if (Helpers.GameManager.UiManager == null) return;

        Helpers.GameManager.LoadSceneManager.LoadLevel(_nextScene);
        if(Helpers.GameManager.Player) Helpers.GameManager.Player.PausePlayer();
    }
}
