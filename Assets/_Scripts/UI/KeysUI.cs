using UnityEngine;
using UnityEngine.UI;
public class KeysUI : MonoBehaviour
{
    [SerializeField] Image _keyImg;

    #region BUILDER
    public KeysUI SetPosition(Vector2 position)
    {
        transform.position = position;
        return this;
    }
    public KeysUI SetButtonSprite(string inputName)
    {
        _keyImg.sprite = InputManager.Instance.GetKeySpriteByName(inputName);
        return this;
    }

    #endregion

    public static void TurnOn(KeysUI k)
    {
        k.gameObject.SetActive(true);
    }
    public static void TurnOff(KeysUI k)
    {
        if (k) k.gameObject.SetActive(false);
    }

    public void ReturnObject()
    {
        FRY_KeysUI.Instance.ReturnObject(this);
    }
}
