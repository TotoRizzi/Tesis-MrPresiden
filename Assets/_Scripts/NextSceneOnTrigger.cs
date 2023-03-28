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
        Helpers.GameManager.OnSpiked += () => StartCoroutine(HideExit());

        _collider = GetComponent<Collider2D>();
        _anim = GetComponentInChildren<Animator>();
        StartCoroutine(HideExit());
    }
    void ShowExit()
    {
        _collider.enabled = true;
        lightParent.SetActive(true);
        _anim.Play("Open");
    }
    IEnumerator HideExit()
    {
        yield return new WaitForEndOfFrame();
        lightParent.SetActive(false);
        _collider.enabled = false;
        _anim.Play("Closed");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Aca hay que ponerle play a la cinematica de victoria

        if (_isTutorial) return;
        if (Helpers.GameManager.UiManager == null) return;

        Helpers.GameManager.LoadSceneManager.LoadLevel(_nextScene);
        Helpers.GameManager.UiManager.CloseCurtain();
        if(Helpers.GameManager.Player) Helpers.GameManager.Player.PausePlayer();
    }
}
