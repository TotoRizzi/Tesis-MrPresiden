using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialVillainText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textDispay;
    [SerializeField] string _sentence;
    [SerializeField] float _typingSpeed;

    private void Start()
    {
        StartCoroutine(Type());
    }

    IEnumerator Type()
    {
        var wait = new WaitForSeconds(_typingSpeed);
        foreach (char letter in _sentence)
        {
            _textDispay.text += letter;
            yield return wait;
        }
    }
}
