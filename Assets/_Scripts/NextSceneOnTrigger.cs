using UnityEngine;
using System.Collections;
public class NextSceneOnTrigger : MonoBehaviour
{
    [SerializeField] bool _isTutorial;
    [SerializeField] int _nextScene;
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
        //Aca hay que ponerle play a la cinematica de victoria

        if (_isTutorial) return;
        if (Helpers.GameManager.UiManager == null) return;

        Helpers.GameManager.LoadSceneManager.LoadLevel(_nextScene);
        //Helpers.GameManager.UiManager.CloseCurtain();
        if(Helpers.GameManager.Player) Helpers.GameManager.Player.PausePlayer();
    }
}
