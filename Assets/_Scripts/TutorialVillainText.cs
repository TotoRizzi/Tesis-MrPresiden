using System.Collections;
using UnityEngine;
using TMPro;
public class TutorialVillainText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textDispay;
    [SerializeField] Transform _eyesParent;
    [SerializeField] float _typingSpeed;
    [SerializeField] float _eyesSpeed;
    [SerializeField] float _minClampX, _maxClampX, _minClampY, _maxClampY;
    string _sentence;

    Transform _player;
    Vector3 _initialPos;
    private void Start()
    {
        StartCoroutine(Type());
        _player = Helpers.GameManager.Player.transform;
        _initialPos = _eyesParent.position;
    }
    private void LateUpdate()
    {
        _eyesParent.position = Vector3.Lerp(_eyesParent.position, new Vector3(Mathf.Clamp(_player.position.x, _initialPos.x + _minClampX, _initialPos.x + _maxClampX), Mathf.Clamp(_player.position.y, _initialPos.y + _minClampY, _initialPos.y + _maxClampY), _eyesParent.position.z),Time.deltaTime * _eyesSpeed);
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
