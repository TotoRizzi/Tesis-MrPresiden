using UnityEngine;
using TMPro;
public class TutorialKeysText : MonoBehaviour
{
    [SerializeField] string _keyName;
    TextMeshProUGUI _text;
    InputManager _inputManager;
    void Start()
    {
        _inputManager = InputManager.Instance;
        _text = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        _text.text = _inputManager.KeyNameForButton(_keyName);     
    }
}
