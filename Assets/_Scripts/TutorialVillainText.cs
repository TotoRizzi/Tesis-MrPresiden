using System.Collections;
using UnityEngine;
using TMPro;
public class TutorialVillainText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textDispay;
    [SerializeField] Transform _eyesParent;
    [SerializeField] Transform[] _eyes;
    [SerializeField] float _typingSpeed;
    string _sentence;

    Transform _player;
    Vector3[] _initialPos = new Vector3[2];
    private void Start()
    {
        StartCoroutine(Type());
        _player = Helpers.GameManager.Player.transform;

        for (int i = 0; i < _initialPos.Length; i++) _initialPos[i] = _eyes[i].position;
    }
    private void LateUpdate()
    {
        Vector3 dist = _player.position - _eyesParent.position;

        for (int i = 0; i < _eyes.Length; i++)
            _eyes[i].right = dist;
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
