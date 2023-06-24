using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class KeysUI : MonoBehaviour
{
    [SerializeField] Image _keyImg;
    Sprite _mainSprite, _pressedSprite;

    public bool active { get; set; }
    IEnumerator PressedAnimation()
    {
        var waitForSeconds = new WaitForSeconds(.5f);

        while (true)
        {
            _keyImg.sprite = _mainSprite;
            yield return waitForSeconds;
            _keyImg.sprite = _pressedSprite;
            yield return waitForSeconds;
        }
    }

    #region BUILDER
    public KeysUI SetPosition(Vector2 position)
    {
        transform.position = position;
        return this;
    }
    public KeysUI SetButtonSprite(string inputName)
    {
        _mainSprite = InputManager.Instance.GetKeySpriteByName(inputName);
        _pressedSprite = InputManager.Instance.GetPressedKeySpriteByName(inputName);
        if (active) StartCoroutine(PressedAnimation());
        return this;
    }
    private void Reset()
    {
        active = false;
        StopCoroutine(PressedAnimation());
        _keyImg.sprite = null;
    }
    #endregion
    public static void TurnOn(KeysUI k)
    {
        k.active = true;
        k.gameObject.SetActive(true);
    }
    public static void TurnOff(KeysUI k)
    {
        if (k)
        {
            k.Reset();
            k.gameObject.SetActive(false);
        }
    }

    public void ReturnObject()
    {
        FRY_KeysUI.Instance.ReturnObject(this);
    }
}
