using UnityEngine;
using DG.Tweening;
public class IntroScene : MonoBehaviour
{
    [SerializeField] RectTransform _logoImgRectTransform;

    private void Start()
    {
        var scale = 1000;
        DOTween.To(() => scale, x => scale = x, 1500, 3f).
            OnUpdate(() => _logoImgRectTransform.sizeDelta = new Vector2(scale, scale)).
            OnComplete(() => Helpers.GameManager.LoadSceneManager.LoadLevelAsync("Menu"));
    }
}
