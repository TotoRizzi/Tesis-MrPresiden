using UnityEngine;
using TMPro;
public class TextToTranslate : MonoBehaviour
{
    [SerializeField] string _ID;
    [SerializeField] TextMeshProUGUI _myView;

    public string ID { get { return _ID; } set { _ID = value; } }
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
        string text = LanguageManager.Instance.GetTranslate(_ID).Replace("-", ",");
        _myView.text = text;
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
