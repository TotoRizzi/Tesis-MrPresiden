using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CosmeticShopItem : MonoBehaviour
{
    public CosmeticData CosmeticData { get { return _cosmeticData; } }

    [SerializeField] CosmeticData _cosmeticData;

    [SerializeField] Image _cosmeticImg;
    [SerializeField] TextMeshProUGUI _cosmeticNameTxt;
    public void SetCosmeticData(CosmeticData cosmeticData, Sprite headSprite)
    {
        _cosmeticData = cosmeticData;

        _cosmeticImg.sprite = headSprite;
        _cosmeticNameTxt.text = _cosmeticData.cosmeticName;
    }

    public void SetCosmetics(ref Image headSprite, ref Image torsoSprite, ref Image rightLegSprite, ref Image leftLegSprite, ref Image rightHandSprite, ref Image leftHandSprite)
    {
        headSprite.sprite = _cosmeticData.headSprite;
        torsoSprite.sprite = _cosmeticData.torsoSprite;
        rightLegSprite.sprite = _cosmeticData.rightLegSprite;
        leftLegSprite.sprite = _cosmeticData.leftLegSprite;
        rightHandSprite.sprite = _cosmeticData.rightHandSprite;
        leftHandSprite.sprite = _cosmeticData.leftHandSprite;
    }
}
