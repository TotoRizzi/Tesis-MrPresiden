using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialVillainText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textDispay;
    [SerializeField] Transform[] _eyes;
    [SerializeField] float _typingSpeed;
    [SerializeField] float _smooth;
    string _sentence;

    Transform _player;
    Vector3[] _initialPos;
    private void Start()
    {
        StartCoroutine(Type());
        _player = Helpers.GameManager.Player.transform;
        _initialPos = new Vector3[2] { _eyes[0].position, _eyes[1].position };
    }
    private void LateUpdate()
    {
        Vector3 distance = _player.position - transform.position;

        for (int i = 0; i < _eyes.Length; i++)
            _eyes[i].position = Vector3.Lerp(_eyes[i].position, new Vector3(Mathf.Clamp(distance.x, _initialPos[i].x  + -.25f, _initialPos[i].x + .25f), Mathf.Clamp(distance.y, _initialPos[i].y + - .35f, _initialPos[i].x + .35f), _eyes[i].position.z), _smooth * Time.deltaTime);
    }
    IEnumerator Type()
    {
        var wait = new WaitForSeconds(_typingSpeed);
        yield return new WaitUntil(() => _textDispay.text != string.Empty);
        _sentence = _textDispay.text.Replace("-", ",");
        _textDispay.text = string.Empty;
        foreach (char letter in _sentence)
        {
            _textDispay.text += letter;
            yield return wait;
        }
    }
}
