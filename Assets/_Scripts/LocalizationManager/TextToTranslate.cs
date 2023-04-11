using UnityEngine;
using TMPro;
public class TextToTranslate : MonoBehaviour
{
    [SerializeField] string _ID;
    [SerializeField] TextMeshProUGUI _myView;
    private void Awake()
    {
        LanguageManager.Instance.OnUpdate += ChangeLang;
    }
    private void Start()
    {
        ChangeLang();
    }
    void ChangeLang()
    {
        _myView.text = LanguageManager.Instance.GetTranslate(_ID);
    }
    private void OnEnable()
    {
        LanguageManager.Instance.OnUpdate += ChangeLang;
    }
    private void OnDisable()
    {
        LanguageManager.Instance.OnUpdate -= ChangeLang;
    }
    private void OnDestroy()
    {
        LanguageManager.Instance.OnUpdate -= ChangeLang;
    }
}
